using System.Linq;
using BallDuel.Scenes.Shared;
using Godot;

public partial class GoldenBallScene : BaseScene
{
    private static string ballScenePath = "res://Scenes/Shared/ball.tscn";

    private PackedScene _ballScene = GD.Load<PackedScene>(ballScenePath);

    public override void _Ready()
    {
        base._Ready();

        BlockingMessageController.Init(this);
        
        // var tethers = GetChildren().OfType<TetherBall>().ToArray();
        // foreach (var tether in tethers)
        // {
        // }

        // CountdownController.Init(this);
        // CountdownController.StartCountdown();
    }

    public override void ResetScene()
    {
        base.ResetScene();

        var tethers = GetChildren().OfType<TetherBall>().ToArray();
        foreach (var tether in tethers) tether.ResetToStart();

        var scoreLabels = GetChildren().OfType<RichTextLabel>()
            .Where(l => l.Name.ToString().Contains("Score"));
        foreach (var scoreLabel in scoreLabels)
            scoreLabel.Text = "0";

        BlockingMessageController.HideBlockingMessage();
    }

    public void BallEnteredPlayer1Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "BlueScore");
    public void BallEnteredPlayer2Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "RedScore");
    public void BallEnteredPlayer3Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "GreenScore");
    public void BallEnteredPlayer4Goal(Node2D body) => UpdateScoreAndResetGoldenBall(body, "YellowScore");

    private void UpdateScoreAndResetGoldenBall(Node2D body, NodePath scoreName)
    {
        if (body is Ball ball && ball.GetName().ToString() == "GoldenBall")
        {
            var blueScore = GetNode<RichTextLabel>(scoreName);
            var newScore = int.Parse(blueScore.Text) + 1;
            blueScore.Text = newScore.ToString();

            ball.ResetToStart();
            CheckForWin();
        }
    }

    private void CheckForWin()
    {
        var blueScore = int.Parse(GetNode<RichTextLabel>("BlueScore").Text);
        var redScore = int.Parse(GetNode<RichTextLabel>("RedScore").Text);
        var greenScore = int.Parse(GetNode<RichTextLabel>("GreenScore").Text);
        var yellowScore = int.Parse(GetNode<RichTextLabel>("YellowScore").Text);

        const int winScore = 3;
        if (blueScore == winScore) BlockingMessageController.ShowBlockingMessage($"Blue wins!");
        if (redScore == winScore) BlockingMessageController.ShowBlockingMessage($"Red wins!");
        if (greenScore == winScore) BlockingMessageController.ShowBlockingMessage($"Green wins!");
        if (yellowScore == winScore) BlockingMessageController.ShowBlockingMessage($"Yellow wins!");
    }
}