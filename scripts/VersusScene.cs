using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class VersusScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    List<PlayerBall> playerBallList = new List<PlayerBall>();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("VersusScene ready");

        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        if (!playerBall1.IsControllerConnected())
        {
            playerBall1.Position = new Vector2(100000, 100000);
        }
        else
        {
            playerBallList.Add(playerBall1);
        }

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }
        else
        {
            playerBallList.Add(playerBall2);
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            return;
        }

        if (playerBall1 != null && playerBall1.IsControllerConnected() && playerBall1.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 1");
            playerBall1.MoveBody(new Vector2(-400, -200));
        }

        if (playerBall2 != null && playerBall2.IsControllerConnected() && playerBall2.Position.X > 50000)
        {
            Console.WriteLine("Connecting player 2");
            playerBall2.MoveBody(new Vector2(400, -200));
        }
    }

    public void CheckForWin()
    {
        Console.WriteLine("CheckForWin");
        var remainingPlayerList = playerBallList.Where(b => b.Position.X > -50000).ToList();
        if (remainingPlayerList.Count == 1)
        {
            var remainingPlayer = remainingPlayerList[1];
            // give score
            Console.WriteLine("player " + remainingPlayer.ControllerId + " wins");
        } else if (remainingPlayerList.Count == 0)
        {
            // draw
            Console.WriteLine("draw");
        }
    }
}