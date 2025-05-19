using System;
using System.Collections.Generic;
using BallDuel.Scenes.AirHockey;
using Godot;

public partial class AirHockeyScene : Node2D
{
    PlayerBall playerBall1 = null;
    PlayerBall playerBall2 = null;
    PlayerBall playerBall3 = null;
    PlayerBall playerBall4 = null;
    BallDuel.Scenes.AirHockey.Puck puck = null;
    List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());
        
        puck = GetNode<BallDuel.Scenes.AirHockey.Puck>("Puck");

        playerBall1 = GetNode<PlayerBall>("PlayerBall1");
        playerBall1.OriginalPosition = new Vector2(-400, -100);
        playerBallList.Add(playerBall1);
        if (!playerBall1.IsControllerConnected())
        {
            playerBall1.Position = new Vector2(100000, 100000);
        }

        playerBall2 = GetNode<PlayerBall>("PlayerBall2");
        playerBall2.OriginalPosition = new Vector2(400, -100);
        playerBallList.Add(playerBall2);
        if (!playerBall2.IsControllerConnected())
        {
            playerBall2.Position = new Vector2(100000, 100000);
        }

        playerBall3 = GetNode<PlayerBall>("PlayerBall3");
        playerBall3.OriginalPosition = new Vector2(-400, 100);
        playerBallList.Add(playerBall3);
        if (!playerBall3.IsControllerConnected())
        {
            playerBall3.Position = new Vector2(100000, 100000);
        }

        playerBall4 = GetNode<PlayerBall>("PlayerBall4");
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
            ResetScene();
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

    public void ResetScene()
    {
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.Reset();
        }
        puck.Reset();
    }
    
    public void BlueGoal(Node2D body)
    {
        if (body is Puck ball)
        {
            var RightScore = GetNode<RichTextLabel>("RightScore");
            RightScore.Text = (int.Parse(RightScore.Text) + 1).ToString();
            
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