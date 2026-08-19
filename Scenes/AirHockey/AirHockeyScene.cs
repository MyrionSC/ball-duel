using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.AirHockey;

public partial class AirHockeyScene : BaseScene
{
    private Puck _puck;

    public override void _Ready()
    {
        base._Ready();

        _puck = GetNode<Puck>("Puck");

        BlockingMessageController.Init(this);
        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _puck.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        CountdownController.StartCountdown();
        _puck.Reset();
    }

    private void BlueGoal(Node2D body)
    {
        if (body is Puck puck)
        {
            var rightScore = GetNode<RichTextLabel>("RightScore");
            var newScore = int.Parse(rightScore.Text) + 1;
            rightScore.Text = newScore.ToString();

            if (newScore >= 5)
            {
                Globals.InputDisabled = true;
                BlockingMessageController.ShowBlockingMessage($"Red wins!");
            }
            else
            {
                StartNextRound();
            }
        }
    }

    private void RedGoal(Node2D body)
    {
        if (body is Puck puck)
        {
            var leftScore = GetNode<RichTextLabel>("LeftScore");
            var newScore = int.Parse(leftScore.Text) + 1;
            leftScore.Text = newScore.ToString();
            
            if (newScore >= 5)
            {
                Globals.InputDisabled = true;
                BlockingMessageController.ShowBlockingMessage("Blue wins!");
            }
            else
            {
                StartNextRound();
            }
        }
    }
}