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

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        // playerBall1.OriginalPosition = new Vector2(-600, -300);
        playerBallList.Add(playerBall1);

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        // playerBall2.OriginalPosition = new Vector2(-600, -210);
        playerBallList.Add(playerBall2);

        playerBall3 = GetNode<PlayerBall>("PlayerBall3");
        // playerBall3.OriginalPosition = new Vector2(-600, -120);
        playerBallList.Add(playerBall3);

        playerBall4 = GetNode<PlayerBall>("PlayerBall4");
        // playerBall4.OriginalPosition = new Vector2(-600, -30);
        playerBallList.Add(playerBall4);

        // foreach (var playerBall in playerBallList)
        // {
        //     playerBall.IsRespawning = false;
        //     if (!playerBall.IsControllerConnected())
        //     {
        //         playerBall.Position = new Vector2(100000, 100000);
        //     }
        // }

        // BlockingMessageController.Init(this);
        // BlockingMessageController.HideBlockingMessage();

        // CountdownController.Init(this);
        // CountdownController.StartCountdown();
        
        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall ball)
            {
                GetTree().CreateTimer(1).Timeout += () =>
                {
                    ball.ResetPosition();
                };
            }
        };
        
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
            CountdownController.StartCountdown();
        }
    }
}