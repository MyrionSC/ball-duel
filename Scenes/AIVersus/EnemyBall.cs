using System;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.AIVersus;

public partial class EnemyBall : RigidBody2D
{
    private bool _resetState = false;
    public Vector2 OriginalPosition = Vector2.Zero;
    private Vector2 _newPosition;
    public bool IsRespawning = false;
    private Line2D _line;
    public bool ShouldDrawLine = true;
    public PlayerBall playerBall = null;

    [Export] public int ControllerId { get; set; } = 0; // Default to first controller

    public override void _Ready()
    {
        base._Ready();
        OriginalPosition = Position;

        _line = new Line2D();
        var r = new Random();
        _line.DefaultColor = Color.Color8((byte)(r.NextInt64() % 256), (byte)(r.NextInt64() % 256),
            (byte)(r.NextInt64() % 256));
        _line.Width = 1.5f;
        var scene = GetParent<Node2D>();
        scene.CallDeferred("add_child", _line);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        if (_resetState)
        {
            _resetState = false;
            Position = _newPosition;
            LinearVelocity = Vector2.Zero;
            return;
        }

        var dirVector = playerBall.GetPosition() - GetPosition();
        var forceVector = dirVector.Normalized() * Globals.BALL_FORCE_MULTIPLIER_CONSTANT * Globals.BALL_ACCELERATION_CONSTANT;
        
        if (ShouldDrawLine)
        {
            _line.ClearPoints();
            _line.AddPoint(GetPosition());
            _line.AddPoint(GetPosition() + forceVector);
        }
        
        ApplyForce(forceVector);
    }

    public void ResetPosition()
    {
        MoveBody(OriginalPosition);
    }

    public void MoveBody(Vector2 targetPosition)
    {
        _resetState = true;
        _newPosition = targetPosition;
    }
}