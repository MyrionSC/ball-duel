using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.CrabBucket;

public partial class CrabBucketScene : BaseScene
{
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
                GetTree().CreateTimer(1).Timeout += () => { ball.ResetPosition(); };
            }
        };
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is not PlayerBall ball) return;
        
        foreach (var playerBall in playerBallList)
        {
            if (playerBall.IsControllerConnected())
                playerBall.ResetPosition();
        }

        var scoreLabel = GetNode<RichTextLabel>(ball.GetColorName() + "Score");
        var score = int.Parse(scoreLabel.Text) + 1;
        scoreLabel.Text = score.ToString();

        if (score >= 3)
        {
            Globals.InputDisabled = true;
            BlockingMessageController.ShowBlockingMessage($"{ball.GetColorName()} wins!");
        }
        else
        {
            CountdownController.StartCountdown();
        }
    }
    
}