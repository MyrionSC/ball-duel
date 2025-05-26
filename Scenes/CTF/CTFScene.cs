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
    public CTFBlueGoal BlueGoal = null;
    public CTFRedGoal RedGoal = null;

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        BlueGoal = GetNode<CTFBlueGoal>("BlueGoal");
        RedGoal = GetNode<CTFRedGoal>("RedGoal");

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
        playerBall2.flagSprite = GetNode<Sprite2D>("PlayerBall2/Player2BlueFlag");
        playerBallList.Add(playerBall2);
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }

        playerBall3 = GetNode<PlayerBallWithFlag>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(-400, 100);
        playerBall3.flagSprite = GetNode<Sprite2D>("PlayerBall3/Player3RedFlag");
        playerBallList.Add(playerBall3);
        if (!playerBall3.IsControllerConnected())
        {
            playerBall3.Position = new Vector2(100000, 100000);
        }

        playerBall4 = GetNode<PlayerBallWithFlag>("PlayerBall4");
        playerBall4.OriginalPosition = new Vector2(400, 100);
        playerBall4.flagSprite = GetNode<Sprite2D>("PlayerBall4/Player4BlueFlag");
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

        // if ball is blue and has red flag and blue flag is here, increment score
        if (ballWithFlag.isBlue())
        {
            if (ballWithFlag.HasFlag() && BlueGoal.flagSprite.IsVisible())
            {
                // inc score
                var leftScore = GetNode<RichTextLabel>("LeftScore");
                leftScore.Text = (int.Parse(leftScore.Text) + 1).ToString();
                ballWithFlag.SetHasFlag(false);
                RedGoal.flagSprite.SetVisible(true);
            }
        }
        else
        {
            // if ball is red and flag is here, pick up flag
            if (BlueGoal.flagSprite.IsVisible())
            {
                ballWithFlag.SetHasFlag(true);
                BlueGoal.flagSprite.SetVisible(false);
            }
        }
    }

    public void BallTouchedRedGoal(PlayerBallWithFlag ballWithFlag)
    {
        Console.WriteLine("BallTouchedRedGoal: " + ballWithFlag.ControllerId);

        // if ball is blue and flag is here, pick up flag
        if (ballWithFlag.isBlue())
        {
            if (RedGoal.flagSprite.IsVisible())
            {
                ballWithFlag.SetHasFlag(true);
                RedGoal.flagSprite.SetVisible(false);
            }
        }
        else
        {
            if (ballWithFlag.HasFlag() && RedGoal.flagSprite.IsVisible())
            {
                // inc score
                var rightScore = GetNode<RichTextLabel>("RightScore");
                rightScore.Text = (int.Parse(rightScore.Text) + 1).ToString();
                ballWithFlag.SetHasFlag(false);
                BlueGoal.flagSprite.SetVisible(true);
            }
        }
    }

    private void ResetVersus()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
            {
                playerBall.Reset();
                RedGoal.flagSprite.SetVisible(true);
                BlueGoal.flagSprite.SetVisible(true);
            }
        }
    }
}