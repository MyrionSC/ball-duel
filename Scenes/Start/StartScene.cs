using System;
using System.Collections.Generic;
using BallDuel.scripts;
using Godot;

public partial class StartScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();
        Globals.InputDisabled = false;
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());
        
        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        playerBall1.OriginalPosition = new Vector2(0, -200);
        playerBallList.Add(playerBall1);

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        playerBall2.OriginalPosition = new Vector2(0, -100);
        playerBallList.Add(playerBall2);

        playerBall3 = GetNode<PlayerBall>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(0, 100);
        playerBallList.Add(playerBall3);

        playerBall4 = GetNode<PlayerBall>("PlayerBall4");
        playerBall4.OriginalPosition = new Vector2(0, 200);
        playerBallList.Add(playerBall4);

        foreach (var playerBall in playerBallList)
        {
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
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
            foreach (var playerBall in playerBallList)
            {
                if (playerBall.IsControllerConnected()) playerBall.ResetPosition();
            }

            return;
        }

        // Console.WriteLine(@event.GetType().Name + ": " + @event.AsText());
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected() && playerBall.Position.X > 50000)
            {
                Console.WriteLine("Connecting playerball " + playerBall.ControllerId);
                playerBall.ResetPosition();
            }
        }
    }
}