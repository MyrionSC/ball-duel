using System;
using BallDuel.Scenes.Shared;
using Godot;

namespace BallDuel.Scenes.ExplodyBall;

public partial class ExplodyBallScene : BaseScene
{
    private ExplodyBall _explodyBall;
    private Random _random = new();

    public override void _Ready()
    {
        base._Ready();

        _explodyBall = GetNode<ExplodyBall>("ExplodyBall");

        var randomAngle = (float)(_random.NextDouble() * Math.PI * 2);
        const float speed = 20f;
        _explodyBall.LinearVelocity = new Vector2(randomAngle * speed, randomAngle * speed);

        BlockingMessageController.Init(this);
        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _explodyBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        CountdownController.StartCountdown();
        _explodyBall.Reset();
    }
}