using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.GoldenBallWithOrbiters;

public partial class GoldenBallWithOrbitersScene : BaseScene
{
    public override void _Ready()
    {
        base._Ready();

        BlockingMessageController.Init(this);

        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    public override void ResetScene()
    {
        base.ResetScene();
        GetNode<Ball>("GoldenBall").ResetToStart();
    }

    public void BallEnteredPlayer1Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "Blue");
    public void BallEnteredPlayer2Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "Red");
    public void BallEnteredPlayer3Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "Green");
    public void BallEnteredPlayer4Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "Yellow");

    private void UpdateScoreAndResetGoldenBall(Node2D body, NodePath colorName)
    {
        if (body is Ball ball && ball.GetName().ToString() == "GoldenBall")
        {
            var scoreNode = GetNode<RichTextLabel>($"{colorName}Score");
            var newScore = int.Parse(scoreNode.Text) + 1;
            scoreNode.Text = newScore.ToString();

            ball.ResetToStart();

            if (newScore >= 5)
            {
                Globals.InputDisabled = true;
                BlockingMessageController.ShowBlockingMessage($"{colorName} wins!");
            }
        }
    }
    
}