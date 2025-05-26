using System;
using System.Collections.Generic;
using BallDuel.Scenes.CTF;
using Godot;

public partial class CTFScene : Node2D
{
    PlayerBallWithFlag playerBall1 = null;
    PlayerBallWithFlag playerBall2 = null;
    PlayerBallWithFlag playerBall3 = null;
    PlayerBallWithFlag playerBall4 = null;

    List<PlayerBallWithFlag> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        playerBall1 = GetNode<PlayerBallWithFlag>("PlayerBall1");
        playerBall1.OriginalPosition = new Vector2(-400, -100);
        playerBall1.flagSprite = GetNode<Sprite2D>("PlayerBall1/Player1RedFlag");
        playerBallList.Add(playerBall1);
        if (!playerBall1.IsControllerConnected())
        {
            playerBall1.Position = new Vector2(100000, 100000);
        }

        playerBall2 = GetNode<PlayerBallWithFlag>("PlayerBall2");
        playerBall2.OriginalPosition = new Vector2(400, -100);
        playerBallList.Add(playerBall2);
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }

        playerBall3 = GetNode<PlayerBallWithFlag>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(-400, 100);
        playerBallList.Add(playerBall3);
        if (!playerBall3.IsControllerConnected())
        {
            playerBall3.Position = new Vector2(100000, 100000);
        }

        playerBall4 = GetNode<PlayerBallWithFlag>("PlayerBall4");
        playerBall4.OriginalPosition = new Vector2(400, 100);
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

        if (@event is InputEventJoypadButton btn1 && btn1.ButtonIndex == JoyButton.Back)
        {
            GetTree().ChangeSceneToFile("res://Scenes/Start/StartScene.tscn");
            return;
        }

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

    public void BallTouchedBlueGoal(PlayerBallWithFlag ballWithFlag)
    {
        Console.WriteLine("BallTouchedBlueGoal: " + ballWithFlag.ControllerId);
    }

    public void BallTouchedRedGoal(PlayerBallWithFlag ballWithFlag)
    {
        Console.WriteLine("BallTouchedRedGoal: " + ballWithFlag.ControllerId);
    }

    private void ResetVersus()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.Reset();
        }
    }
}