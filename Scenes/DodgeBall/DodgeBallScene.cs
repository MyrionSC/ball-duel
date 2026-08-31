using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.DodgeBall;

public partial class DodgeBallScene : BaseScene
{
    private Random _random = new();

    private static readonly string ballScenePath = "res://Scenes/DodgeBall/DodgeBallBall.tscn";
    private PackedScene _ballScene = GD.Load<PackedScene>(ballScenePath);
    private float ballSpawnTime = 2f;
    private List<RigidBody2D> ballList = new();

    public override void _Ready()
    {
        base._Ready();

        BlockingMessageController.Init(this);

        CountdownController.Init(this);
        CountdownController.StartCountdown();

        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall ball)
            {
                Console.WriteLine("ball: " + ball.ControllerId + " touched border");
                GetTree().CreateTimer(0.1).Timeout += CheckForWin;
            }
        };

        // Spawn ball leftside
        var ballSpawnVelocity = 150;
        int[] lanes = [-910, -405, 100, 595];

        // first wave
        GenerateBallSpawnArray(lanes, ballSpawnVelocity);

        void SpawnBallLoop()
        {
            GenerateBallSpawnArray(lanes, ballSpawnVelocity);
            if (ballSpawnTime > 0.69f) ballSpawnTime -= 0.10f;
            var newTimer = GetTree().CreateTimer(ballSpawnTime);
            newTimer.Timeout += SpawnBallLoop;
        }

        var timer = GetTree().CreateTimer(ballSpawnTime);
        timer.Timeout += SpawnBallLoop;
    }

    private void GenerateBallSpawnArray(int[] lanes, int ballSpawnVelocity)
    {
        List<bool> ballSpawnArray = [true, true, true, true, true];
        int falsePosition = _random.Next(0, 4); // 0-3 to allow two consecutive positions
        ballSpawnArray[falsePosition] = false;
        ballSpawnArray[falsePosition + 1] = false;

        List<PlayerBall> remainingPlayerList =
            playerBallList.Where(b => b.IsControllerConnected() && Math.Abs(b.Position.X) < 50000).ToList();
        foreach (var playerBall in remainingPlayerList)
        {
            var lane = lanes[playerBall.ControllerId];
            for (var i = ballSpawnArray.Count - 1; i >= 0; i--)
            {
                if (ballSpawnArray[i])
                {
                    SpawnBall(lane + i * 86, -450, 0, ballSpawnVelocity);
                }
            }
        }

        void SpawnBall(int x, int y, int dir_x, int dir_y)
        {
            RigidBody2D ball = _ballScene.Instantiate() as RigidBody2D;
            ballList.Add(ball);
            ball.GlobalPosition = new Vector2(x, y);
            ball.LinearVelocity = new Vector2(dir_x, dir_y);
            AddChild(ball);
        }
    }

    public override void ResetScene()
    {
        base.ResetScene();
        ballSpawnTime = 2f;

        foreach (var ball in ballList)
        {
            ball.QueueFree();
        }
        ballList.Clear();


        CountdownController.StartCountdown();
    }

    private void StartNextRound()
    {
        ResetPositions();
        ballSpawnTime = 2f;
        foreach (var ball in ballList)
        {
            ball.QueueFree();
        }
        ballList.Clear();
        CountdownController.StartCountdown();
    }

    private void CheckForWin()
    {
        Console.WriteLine("Checking for win...");

        // TODO: Check for score win

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