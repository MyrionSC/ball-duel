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
    private Random _random = new Random();

    public override void _Ready()
    {
        base._Ready();

        _poisonBall = GetNode<PoisonBall>("PoisonBall");
        
        
        
        
        
        _poisonBall.SetContactMonitor(true);
        _poisonBall.MaxContactsReported = 10;

        InitializePoisonBallVelocity();

        foreach (var playerBall in GetPlayerBalls())
        {
            playerBall.SetContactMonitor(true);
            playerBall.MaxContactsReported = 10;
        }

        BlockingMessageController.Init(this);

        // CountdownController.Init(this);
        // CountdownController.StartCountdown();
    }

    private void InitializePoisonBallVelocity()
    {
        var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        const float speed = 50f;
        _poisonBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _poisonBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        InitializePoisonBallVelocity();
        // CountdownController.StartCountdown();
        _poisonBall.Reset();
    }

    private int _ballSpawnCounter = 0;
    private int _ballSpawnTime = 600;

    public override void _PhysicsProcess(double delta)
    {
        _ballSpawnCounter++;
        if (_ballSpawnCounter >= _ballSpawnTime)
        {
            _ballSpawnCounter = 0;
            Console.WriteLine("SPAWN BALL!");
            SpawnPoisonBallCopy();
        }
    }

    private void SpawnPoisonBallCopy()
    {
        _copiedPoisonBall = _poisonBall.Duplicate() as PoisonBall;
        if (_copiedPoisonBall != null)
        {
            AddChild(_copiedPoisonBall);
            _copiedPoisonBall.Position = _poisonBall.Position;
            _copiedPoisonBall.SetContactMonitor(true);
            _copiedPoisonBall.MaxContactsReported = 10;

            var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
            var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
            const float speed = 50f;
            _copiedPoisonBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);
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