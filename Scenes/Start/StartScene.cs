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
        
        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBallList.Add(playerBall);
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