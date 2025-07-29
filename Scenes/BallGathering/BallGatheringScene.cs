using System;
using BallDuel.Scenes.Shared;
using Godot;

public partial class BallGatheringScene : BaseScene
{
    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());
        
        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBallList.Add(playerBall);
            if (!playerBall.IsControllerConnected())
                playerBall.Position = new Vector2(100000, 100000);
        }

        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    // public override void ResetScene()
    // {
    //     base.ResetScene();
    // }
    

    // public void ResetScene()
    // {
    //     foreach (var playerBall in playerBallList)
    //     {
    //         if (playerBall.IsControllerConnected())
    //             playerBall.ResetPosition();
    //     }
    //     CountdownController.StartCountdown();
    // }

    public void BlueGoal(Node2D body)
    {
    }

    public void RedGoal(Node2D body)
    {
    }
}