using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.CrabBucket;

public partial class CrabBucketScene : Node2D
{
    List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();

        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBallList.Add(playerBall);
            playerBall.IsRespawning = false;
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
        }

        BlockingMessageController.Init(this);

        CountdownController.Init(this);
        CountdownController.StartCountdown();

        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall ball)
            {
                GetTree().CreateTimer(1).Timeout += () => { ball.ResetPosition(); };
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
            BlockingMessageController.HideBlockingMessage();
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

    public void OnBodyEntered(Node2D body)
    {
        if (body is not PlayerBall ball) return;

        ball.ResetPosition();

        var scoreLabel = GetNode<RichTextLabel>(ball.GetColorName() + "Score");
        var oldScore = int.Parse(scoreLabel.Text);
        var score = oldScore + 1;
        scoreLabel.Text = score.ToString();
        
        if (score >= 3)
        {
            Globals.InputDisabled = true;
            BlockingMessageController.ShowBlockingMessage($"{ball.GetColorName()} wins!");
        }
    }

    private void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected()) playerBall.ResetPosition();
        }

        var scoreLabels = GetChildren().OfType<RichTextLabel>().Where(l => l.Name.ToString().Contains("Score"));
        foreach (var scoreLabel in scoreLabels)
            scoreLabel.Text = "0";

        CountdownController.StartCountdown();
    }
}