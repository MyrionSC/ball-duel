using System;
using System.Collections.Generic;
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
                if (playerBall.IsControllerConnected()) playerBall.Reset();
            }

            return;
        }

        // Console.WriteLine(@event.GetType().Name + ": " + @event.AsText());
        if (playerBall1 != null && playerBall1.IsControllerConnected() && playerBall1.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 1");
            playerBall1.Reset();
        }

        if (playerBall2 != null && playerBall2.IsControllerConnected() && playerBall2.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 2");
            playerBall2.Reset();
        }

        if (playerBall3 != null && playerBall3.IsControllerConnected() && playerBall3.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 3");
            playerBall3.Reset();
        }

        if (playerBall4 != null && playerBall4.IsControllerConnected() && playerBall4.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 4");
            playerBall4.Reset();
        }
    }
}