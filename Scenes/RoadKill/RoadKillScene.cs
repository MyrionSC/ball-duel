using System;
using System.Collections.Generic;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.RoadKill;

public partial class RoadKillScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    List<PlayerBall> playerBallList = new();

    private static string roadKillBallScenePath = "res://Scenes/RoadKill/RoadKillBall.tscn";
    private PackedScene _ballScene = GD.Load<PackedScene>(roadKillBallScenePath);

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBallList.Add(playerBall);
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
        }

        BlockingMessageController.Init(this);
        BlockingMessageController.HideBlockingMessage();

        // CountdownController.Init(this);
        // CountdownController.StartCountdown();

        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall ball)
            {
                GetTree().CreateTimer(1).Timeout += () => { ball.ResetPosition(); };
            }

            if (body is RigidBody2D roadKillBall && roadKillBall.SceneFilePath == roadKillBallScenePath)
            {
                RemoveChild(roadKillBall);
            }
        };

        
        // === SPAWN BALL LANES ===
        void SpawnBall(int x, int y, int dir_x, int dir_y)
        {
            RigidBody2D ball = _ballScene.Instantiate() as RigidBody2D;
            ball.GlobalPosition = new Vector2(x, y);
            ball.LinearVelocity = new Vector2(dir_x, dir_y);
            AddChild(ball);
        }

        // Spawn ball leftside
        foreach (var y in new[] { -200, 000, 200 })
        {
            void SpawnBallLaneLoop()
            {
                SpawnBall(-750, y, 200, 0);
                var newTimer = GetTree().CreateTimer(0.75f);
                newTimer.Timeout += SpawnBallLaneLoop;
            }

            var timer = GetTree().CreateTimer(0.75f);
            timer.Timeout += SpawnBallLaneLoop;
        }

        // Spawn ball rightside
        foreach (var y in new[] { -100, 100 })
        {
            void SpawnBallLaneLoop()
            {
                SpawnBall(750, y, -200, 0);
                var newTimer = GetTree().CreateTimer(0.75f);
                newTimer.Timeout += SpawnBallLaneLoop;
            }

            var timer = GetTree().CreateTimer(0.75f);
            timer.Timeout += SpawnBallLaneLoop;
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            return;
        }

        if (@event is InputEventJoypadButton btn && btn.ButtonIndex == JoyButton.Start)
        {
            ResetScene();
            BlockingMessageController.HideBlockingMessage();
            return;
        }

        if (@event is InputEventJoypadButton btn1 && btn1.ButtonIndex == JoyButton.Back)
        {
            GetTree().ChangeSceneToFile("res://Scenes/Start/StartScene.tscn");
            return;
        }

        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected() && playerBall.Position.X > 50000)
            {
                Console.WriteLine("Connecting playerball " + playerBall.ControllerId);
                playerBall.ResetPosition();
            }
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBall ball)
        {
            ball.ResetPosition();
            Globals.InputDisabled = true;
            BlockingMessageController.ShowBlockingMessage($"{ball.GetColorName()} ball wins!");
        }
    }

    private void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
            if (CountdownController.IsInitted) CountdownController.StartCountdown();
        }
    }
}