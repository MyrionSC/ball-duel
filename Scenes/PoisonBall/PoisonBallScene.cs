using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.PoisonBall;

public partial class PoisonBallScene : BaseScene
{
    private PoisonBall _poisonBall;
    private PoisonBall _copiedPoisonBall;
    private Random _random = new();
    private int _ballSpawnCounter = 0;
    private int _ballSpawnTime = 400;
    private const float TrackingForce = 50f;
    private Area2D _poisonBallSpawn;
    private Vector2 _viewportSize;
    private List<PoisonBall> _poisonBallList = new();

    public override void _Ready()
    {
        base._Ready();

        _poisonBall = GetNode<PoisonBall>("PoisonBall");
        _poisonBallSpawn = GetNode<Area2D>("PoisonBallSpawn");

        _viewportSize = GetViewportRect().Size;
        RandomizePoisonBallSpawnPosition();

        _poisonBall.SetContactMonitor(true);
        _poisonBall.MaxContactsReported = 10;

        InitializePoisonBallVelocity();

        foreach (var playerBall in GetPlayerBalls())
        {
            playerBall.SetContactMonitor(true);
            playerBall.MaxContactsReported = 10;
        }

        _poisonBallList = new List<PoisonBall>();
        _poisonBallList.Add(_poisonBall);

        BlockingMessageController.Init(this);

        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    private void InitializePoisonBallVelocity()
    {
        var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        const float speed = 50f;
        _poisonBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);
    }

    private void RandomizePoisonBallSpawnPosition()
    {
        var randomX = (float)(_random.NextDouble() * (_viewportSize.X - 200) - (_viewportSize.X - 200) / 2);
        var randomY = (float)(_random.NextDouble() * (_viewportSize.Y - 200) - (_viewportSize.Y - 200) / 2);
        _poisonBallSpawn.Position = new Vector2(randomX, randomY);
    }

    public override void ResetScene()
    {
        base.ResetScene();
        StartNextRound();
    }

    private void StartNextRound()
    {
        ResetPositions();
        InitializePoisonBallVelocity();

        _ballSpawnCounter = 0;

        foreach (var ball in _poisonBallList)
        {
            if (ball != _poisonBall)
                ball.QueueFree();
        }

        _poisonBallList.Clear();
        _poisonBallList.Add(_poisonBall);
        _poisonBall.Reset();

        RandomizePoisonBallSpawnPosition();

        CountdownController.StartCountdown();
    }

    private Vector2 FindClosestPlayerBall(Vector2 position)
    {
        PlayerBall closest = null;
        float minDistance = float.MaxValue;

        foreach (var playerBall in GetPlayerBalls())
        {
            if (!playerBall.IsControllerConnected() || Math.Abs(playerBall.Position.X) >= 50000)
                continue;

            float distance = position.DistanceTo(playerBall.Position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = playerBall;
            }
        }

        if (closest == null)
            return Vector2.Zero;

        return (closest.Position - position).Normalized();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!Globals.InputDisabled)
        {
            _ballSpawnCounter++;

            foreach (var poisonBall in _poisonBallList)
            {
                var directionToClosest = FindClosestPlayerBall(poisonBall.Position);
                if (directionToClosest != Vector2.Zero)
                {
                    poisonBall.ApplyCentralForce(directionToClosest * TrackingForce);
                }
            }

            if (_ballSpawnCounter >= _ballSpawnTime)
            {
                _ballSpawnCounter = 0;
                SpawnPoisonBallCopy();
            }
        }
    }

    private void SpawnPoisonBallCopy()
    {
        _copiedPoisonBall = _poisonBall.Duplicate() as PoisonBall;
        if (_copiedPoisonBall != null)
        {
            AddChild(_copiedPoisonBall);
            _copiedPoisonBall.Position = _poisonBallSpawn.Position;
            _copiedPoisonBall.SetContactMonitor(true);
            _copiedPoisonBall.MaxContactsReported = 10;

            var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
            var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
            const float speed = 50f;
            _copiedPoisonBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);

            _poisonBallList.Add(_copiedPoisonBall);

            RandomizePoisonBallSpawnPosition();
        }
    }

    public void Collide(Node2D body)
    {
        GD.Print($"Collided with: {body.Name}");
        if (body is PlayerBall playerBall)
        {
            playerBall.MoveBody(new Vector2(-100000, 0));
            GetTree().CreateTimer(0.1).Timeout += CheckForWin;
        }
    }

    private void CheckForWin()
    {
        Console.WriteLine("Checking for win...");

        List<PlayerBall> remainingPlayerList =
            playerBallList.Where(b => b.IsControllerConnected() && Math.Abs(b.Position.X) < 50000).ToList();

        if (remainingPlayerList.Count == 1)
        {
            PlayerBall remainingPlayer = remainingPlayerList[0];
            var scoreLabel = GetNode<RichTextLabel>("Player" + (remainingPlayer.ControllerId + 1) + "Score");
            var oldScore = int.Parse(scoreLabel.Text);
            Console.WriteLine(
                $"player {remainingPlayer.ControllerId} wins, old score: {oldScore} new score: {oldScore + 1}");
            scoreLabel.Text = (oldScore + 1).ToString();
            StartNextRound();
        }
        else if (remainingPlayerList.Count == 0)
        {
            Console.WriteLine("draw");
            StartNextRound();
        }
        else
        {
            Console.WriteLine(remainingPlayerList.Count + " players left");
        }
    }
}