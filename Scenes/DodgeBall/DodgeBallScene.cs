using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.DodgeBall;

public partial class DodgeBallScene : BaseScene
{
    private Random _random = new Random();

    public override void _Ready()
    {
        base._Ready();

        BlockingMessageController.Init(this);
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

    public override void ResetScene()
    {
        base.ResetScene();
    }

    private void StartNextRound()
    {
        ResetPositions();
        CountdownController.StartCountdown();
    }
    
    private void CheckForWin()
    {
        Console.WriteLine("Checking for win...");
        
        // TODO: Check for score win

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
            StartNextRound();
        }
        else if (remainingPlayerList.Count == 0)
        {
            Console.WriteLine("draw");
            StartNextRound();
        }
        else
        {
            Console.WriteLine(remainingPlayerList.Count + " players left");
        }
    }

    
}