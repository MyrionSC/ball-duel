using System;
using System.Collections.Generic;
using BallDuel.Scenes.BallGathering;
using BallDuel.Scenes.Shared;
using Godot;

public partial class BallGatheringScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    Puck puck = null;
    List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        puck = GetNode<Puck>("Puck");

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        playerBall1.OriginalPosition = new Vector2(-400, -100);
        playerBallList.Add(playerBall1);

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        playerBall2.OriginalPosition = new Vector2(400, -100);
        playerBallList.Add(playerBall2);

        playerBall3 = GetNode<PlayerBall>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(-400, 100);
        playerBallList.Add(playerBall3);

        playerBall4 = GetNode<PlayerBall>("PlayerBall4");
        playerBall4.OriginalPosition = new Vector2(400, 100);
        playerBallList.Add(playerBall4);

        foreach (var playerBall in playerBallList)
        {
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
        }

        CountdownController.Init(this);
        CountdownController.StartCountdown();
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

    public void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
        }

        CountdownController.StartCountdown();
        puck.Reset();
    }

    public void BlueGoal(Node2D body)
    {
        if (body is Puck ball)
        {
            var rightScore = GetNode<RichTextLabel>("RightScore");
            rightScore.Text = (int.Parse(rightScore.Text) + 1).ToString();

            ResetScene();
        }
    }

    public void RedGoal(Node2D body)
    {
        if (body is Puck ball)
        {
            var leftScore = GetNode<RichTextLabel>("LeftScore");
            leftScore.Text = (int.Parse(leftScore.Text) + 1).ToString();

            ResetScene();
        }
    }
}