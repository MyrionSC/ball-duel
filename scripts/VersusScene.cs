using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class VersusScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("VersusScene ready");

        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        playerBall1.OriginalPosition = new Vector2(-400, -200);
        playerBallList.Add(playerBall1);
        if (!playerBall1.IsControllerConnected())
        {
            playerBall1.Position = new Vector2(100000, 100000);
        }

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        playerBall2.OriginalPosition = new Vector2(400, -200);
        playerBallList.Add(playerBall2);
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }

        playerBall3 = GetNode<PlayerBall>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(-400, 200);
        playerBallList.Add(playerBall3);
        if (!playerBall3.IsControllerConnected())
        {
            playerBall3.Position = new Vector2(100000, 100000);
        }
        
        playerBall4 = GetNode<PlayerBall>("PlayerBall4");
        playerBall4.OriginalPosition = new Vector2(400, 200);
        playerBallList.Add(playerBall4);
        if (!playerBall4.IsControllerConnected())
        {
            playerBall4.Position = new Vector2(100000, 100000);
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
            ResetVersus();
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

    private void CheckForWin()
    {
        Console.WriteLine("CheckForWin");
        if (playerBallList.Count == 1) return;
        List<PlayerBall> remainingPlayerList = playerBallList.Where(b => b.IsControllerConnected() && Math.Abs(b.Position.X) < 50000).ToList();
        
        if (remainingPlayerList.Count == 1)
        {
            PlayerBall remainingPlayer = remainingPlayerList[0];
            remainingPlayer.Score += 1;
            Console.WriteLine("player " + remainingPlayer.ControllerId + " wins, new score: " + remainingPlayer.Score);
            ResetVersus();
        }
        else if (remainingPlayerList.Count == 0)
        {
            Console.WriteLine("draw");
            ResetVersus();
        }
        else
        {
            Console.WriteLine(remainingPlayerList.Count + " players left");
        }
    }

    private void ResetVersus()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.Reset();
        }
        
        var spinnyThing = GetNode<MiddleSpinnyThing>("MiddleSpinnyThing");
        spinnyThing.Reset();
    }
}