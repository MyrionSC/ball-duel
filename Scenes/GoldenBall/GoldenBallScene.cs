using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using Godot;

public partial class GoldenBallScene : BaseScene
{
    private static string ballScenePath = "res://Scenes/Shared/ball.tscn";

    private PackedScene _ballScene = GD.Load<PackedScene>(ballScenePath);
    // private List<RigidBody2D> ballList = new();
    // private List<PlayerBall> playerBallList = new();

    public override void _Ready()
    {
        base._Ready();

        // foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        // {
        //     var playerBall = GetNode<PlayerBall>(s);
        //     playerBallList.Add(playerBall);
        //     if (!playerBall.IsControllerConnected())
        //         playerBall.Position = new Vector2(100000, 100000);
        // }

        BlockingMessageController.Init(this);

        // CountdownController.Init(this);
        // CountdownController.StartCountdown();

        SpawnBallsInGrid();
    }

    private void SpawnBallsInGrid()
    {
        void SpawnBall(int x, int y)
        {
            RigidBody2D ball = _ballScene.Instantiate() as RigidBody2D;
            ball.GlobalPosition = new Vector2(x, y);
            var sprite = ball.GetNode<Sprite2D>("Sprite2D");
            sprite.Scale = new Vector2(0.2f, 0.2f);
            var collision = ball.GetNode<CollisionShape2D>("Collision");
            collision.Scale = new Vector2(0.2f, 0.2f);
            // ballList.Add(ball);
            AddChild(ball);
        }

        // foreach (var y in Enumerable.Range(0, 8).Select(x => (x * 60) - 210).ToArray())
        // {
        //     foreach (var x in Enumerable.Range(0, 8).Select(x => (x * 60) - 210).ToArray())
        //     {
        //         SpawnBall(x, y);
        //     }
        // }
    }

    public override void ResetScene()
    {
        base.ResetScene();
        // foreach (var ball in ballList)
        //     RemoveChild(ball);

        var tethers = GetChildren().OfType<TetherBall>().ToArray();
        foreach (var tether in tethers) tether.ResetToStart();

        // SpawnBallsInGrid();

        BlockingMessageController.HideBlockingMessage();
    }

    public void BallEnteredPlayer1Goal(Node2D body)
    {
        UpdateScoreAndResetGoldenBall(body, "BlueScore");
    }

    public void BallEnteredPlayer2Goal(Node2D body)
    {
        UpdateScoreAndResetGoldenBall(body, "RedScore");
    }

    public void BallEnteredPlayer3Goal(Node2D body)
    {
        UpdateScoreAndResetGoldenBall(body, "GreenScore");
    }

    public void BallEnteredPlayer4Goal(Node2D body)
    {
        UpdateScoreAndResetGoldenBall(body, "YellowScore");
    }

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