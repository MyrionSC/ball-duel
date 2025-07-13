using System;
using System.Collections.Generic;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.CrashThrough;

public partial class CrashThroughScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    List<PlayerBall> playerBallList = new();

    private static string CrashThroughBallScenePath = "res://Scenes/CrashThrough/CrashThroughBall.tscn";
    private PackedScene _ballScene = GD.Load<PackedScene>(CrashThroughBallScenePath);

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBall.OriginalPosition = playerBall.GetPosition();
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
        };

        // // === SPAWN BALL LANES ===
        // void SpawnBall(int x, int y, int dir_x, int dir_y)
        // {
        //     RigidBody2D ball = _ballScene.Instantiate() as RigidBody2D;
        //     ball.GlobalPosition = new Vector2(x, y);
        //     ball.LinearVelocity = new Vector2(dir_x, dir_y);
        //     AddChild(ball);
        // }

        CrashThroughBall ball = _ballScene.Instantiate() as CrashThroughBall;
        ball.GlobalPosition = new Vector2(0,0);
        ball.OriginalPosition = ball.GetPosition();
        AddChild(ball);
        ball.InitLine(this);
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