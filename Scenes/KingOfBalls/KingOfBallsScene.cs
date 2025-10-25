using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.KingOfBalls;

public partial class KingOfBallsScene : Node2D
{
    protected List<PlayerBall> playerBallList = [];

    public override void _Ready()
    {
        base._Ready();
        playerBallList = GetChildren().OfType<PlayerBall>().ToList();

        foreach (var playerBall in playerBallList)
        {
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
            
            if (playerBall.OntopSprite != null)
                playerBall.OntopSprite.Texture = GD.Load<Texture2D>("res://assets/Crown.png");
        }

        BlockingMessageController.Init(this);
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

    public virtual void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
        }

        CountdownController.StartCountdown();

        Globals.InputDisabled = false;

        var scoreLabels = GetChildren().OfType<RichTextLabel>()
            .Where(l => l.Name.ToString().Contains("Score"));
        foreach (var scoreLabel in scoreLabels)
            scoreLabel.Text = "0";

        BlockingMessageController.HideBlockingMessage();
    }
}