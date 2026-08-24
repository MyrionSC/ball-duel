using System;
using BallDuel.Scenes.Shared;
using Godot;

namespace BallDuel.Scenes.DodgeBall;

public partial class DodgeBallScene : BaseScene
{
    private DodgeBall _dodgeBall;
    private Random _random = new Random();

    public override void _Ready()
    {
        base._Ready();

        _dodgeBall = GetNode<DodgeBall>("DodgeBall");

        InitializeDodgeBallVelocity();

        BlockingMessageController.Init(this);
        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    private void InitializeDodgeBallVelocity()
    {
        var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        const float speed = 50f;
        _dodgeBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _dodgeBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        InitializeDodgeBallVelocity();
        CountdownController.StartCountdown();
        _dodgeBall.Reset();
    }
    
}