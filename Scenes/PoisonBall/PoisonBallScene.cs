using System;
using BallDuel.Scenes.Shared;
using Godot;

namespace BallDuel.Scenes.PoisonBall;

public partial class PoisonBallScene : BaseScene
{
    private PoisonBall _poisonBall;
    private Random _random = new Random();

    public override void _Ready()
    {
        base._Ready();

        _poisonBall = GetNode<PoisonBall>("PoisonBall");

        // _poisonBall.LinearVelocity = Vector2.Down * 100;
        // Set random start velocity
        var randomAngle = (float)(_random.NextDouble() * Math.PI * 2);
        var randomSpeed = 200f;
        _poisonBall.LinearVelocity = new Vector2(randomAngle * randomSpeed, randomAngle * randomSpeed);

        BlockingMessageController.Init(this);
        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _poisonBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        CountdownController.StartCountdown();
        _poisonBall.Reset();
    }
}