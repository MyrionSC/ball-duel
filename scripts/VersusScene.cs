using Godot;
using System;

public partial class VersusScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("VersusScene ready");

        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");

        Console.WriteLine(playerBall1.IsControllerConnected());

        if (!playerBall1.IsControllerConnected())
        {
            playerBall1.Position = new Vector2(100000, 100000);
        }

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            return;
        }

        Console.WriteLine(playerBall1.Position);
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
}