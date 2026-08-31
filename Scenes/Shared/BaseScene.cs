using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.Shared;

public partial class BaseScene : Node2D
{
    protected List<PlayerBall> playerBallList = [];

    public override void _Ready()
    {
        base._Ready();

        // TODO: Does this break something?
        Globals.InputDisabled = false;
        PhysicsServer2D.SetActive(true);

        playerBallList = GetChildren().OfType<PlayerBall>().ToList();

        foreach (var playerBall in playerBallList)
        {
            var scoreLabel = GetNodeOrNull<RichTextLabel>($"Player{playerBall.ControllerId + 1}Score");
            if (scoreLabel != null) scoreLabel.Visible = true;
            if (!playerBall.IsControllerConnected())
            {
                playerBall.Position = new Vector2(100000, 100000);
                if (scoreLabel != null) scoreLabel.Visible = false;
            }
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
                var scoreLabel = GetNode<RichTextLabel>($"Player{playerBall.ControllerId + 1}Score");
                if (scoreLabel != null) scoreLabel.Visible = true;
                playerBall.ResetPosition();
            }
        }
    }

    public virtual void ResetScene()
    {
        // TODO: test around
        Globals.InputDisabled = false;
        BlockingMessageController.HideBlockingMessage();

        ResetPositions();

        var scoreLabels = GetChildren().OfType<RichTextLabel>().Where(l => l.Name.ToString().Contains("Score"));
        foreach (var scoreLabel in scoreLabels)
            scoreLabel.Text = "0";

        CountdownController.StartCountdown();
    }

    public void ResetPositions()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
        }

        var tethers = GetChildren().OfType<TetherBall>().ToArray();
        foreach (var tether in tethers) tether.ResetToStart();
    }
    
    public IEnumerable<PlayerBall> GetPlayerBalls()
    {
        return GetChildren().OfType<PlayerBall>().Where(node => node.Name.ToString().StartsWith("PlayerBall"));
    }

}