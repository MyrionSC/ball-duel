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

        InitializePoisonBallVelocity();

        BlockingMessageController.Init(this);
        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    private void InitializePoisonBallVelocity()
    {
        var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        const float speed = 50f;
        _poisonBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _poisonBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        InitializePoisonBallVelocity();
        CountdownController.StartCountdown();
        _poisonBall.Reset();
    }
    
}