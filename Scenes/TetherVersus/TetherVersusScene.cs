using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using Godot;

namespace BallDuel.Scenes.TetherVersus;

public partial class TetherVersusScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    List<PlayerBall> playerBallList = new();
    Dictionary<PlayerBall, TetherVersusBall> tetherBallMap = new();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        // foreach (var ballNum in new[] { "1", "2", "3", "4" })
        foreach (var ballNum in new[] { "1" })
        {
            var playerBall = GetNode<PlayerBall>("PlayerBall" + ballNum);
            playerBall.OriginalPosition = playerBall.GetPosition();
            playerBallList.Add(playerBall);
            var tetherBall = GetNode<TetherVersusBall>("TetherBall" + ballNum);
            tetherBall.TetherToPlayer(playerBall);
            tetherBallMap.Add(playerBall, tetherBall);
            
            if (!playerBall.IsControllerConnected())
            {
                playerBall.Position = new Vector2(100000, 100000);
            }
            else
            {
                GetNode<RichTextLabel>("Player" + (playerBall.ControllerId + 1) + "Score").Visible = true;
            }
        }

        CountdownController.Init(this);
        CountdownController.StartCountdown();

        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall ball)
            {
                Console.WriteLine("ball: " + ball.ControllerId + " touched border");
                GetTree().CreateTimer(0.1).Timeout += CheckForWin;
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
                GetNode<RichTextLabel>("Player" + (playerBall.ControllerId + 1) + "Score").Visible = true;
                playerBall.ResetPosition();
            }
        }
    }

    private void CheckForWin()
    {
        Console.WriteLine("Checking for win...");

        List<PlayerBall> remainingPlayerList =
            playerBallList.Where(b => b.IsControllerConnected() && Math.Abs(b.Position.X) < 50000).ToList();

        if (remainingPlayerList.Count == 1)
        {
            PlayerBall remainingPlayer = remainingPlayerList[0];
            var scoreLabel = GetNode<RichTextLabel>("Player" + (remainingPlayer.ControllerId + 1) + "Score");
            var oldScore = int.Parse(scoreLabel.Text);
            Console.WriteLine(
                $"player {remainingPlayer.ControllerId} wins, old score: {oldScore} new score: {oldScore + 1}");
            scoreLabel.Text = (oldScore + 1).ToString();
            ResetScene();
        }
        else if (remainingPlayerList.Count == 0)
        {
            Console.WriteLine("draw");
            ResetScene();
        }
        else
        {
            Console.WriteLine(remainingPlayerList.Count + " players left");
        }
    }

    private void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
            tetherBallMap[playerBall].Reset();
            CountdownController.StartCountdown();
        }
    }
}