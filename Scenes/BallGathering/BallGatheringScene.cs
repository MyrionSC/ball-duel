using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using Godot;

public partial class BallGatheringScene : BaseScene
{
    private static string ballScenePath = "res://Scenes/Shared/ball.tscn";
    private PackedScene _ballScene = GD.Load<PackedScene>(ballScenePath);
    private List<RigidBody2D> ballList = new();

    public override void _Ready()
    {
        base._Ready();

        foreach (var s in new[] { "PlayerBall1", "PlayerBall2", "PlayerBall3", "PlayerBall4" })
        {
            var playerBall = GetNode<PlayerBall>(s);
            playerBallList.Add(playerBall);
            // if (!playerBall.IsControllerConnected())
            //     playerBall.Position = new Vector2(100000, 100000);
        }

        CountdownController.Init(this);
        CountdownController.StartCountdown();

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
            ballList.Add(ball);
            AddChild(ball);
        }

        foreach (var y in Enumerable.Range(0, 8).Select(x => (x * 60) - 210).ToArray())
        {
            foreach (var x in Enumerable.Range(0, 8).Select(x => (x * 60) - 210).ToArray())
            {
                SpawnBall(x, y);
            }
        }
    }

    public override void ResetScene()
    {
        base.ResetScene();
        foreach (var ball in ballList)
            RemoveChild(ball);
        SpawnBallsInGrid();
    }


    public void BlueGoal(Node2D body)
    {
    }

    public void RedGoal(Node2D body)
    {
    }
}