using System;
using Godot;

namespace BallDuel.Scenes.AIVersus;

public partial class EnemyBall : RigidBody2D
{
    private bool _resetState = false;
    public Vector2 OriginalPosition = Vector2.Zero;
    private Vector2 _newPosition;
    public bool IsRespawning = false;
    private Line2D _line;

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

        // Vector2 analogInput = new Vector2(
        //     GetClampedJoyAxis(ControllerId, JoyAxis.LeftX),
        //     GetClampedJoyAxis(ControllerId, JoyAxis.LeftY)
        // );
        // float forceMultiplier = Input.IsActionPressed($"device_{ControllerId}_trigger_right")
        //     ? Globals.BALL_FORCE_MULTIPLIER_CONSTANT
        //     : 1f;
        //
        // ApplyForce(Globals.InputDisabled
        //     ? Vector2.Zero
        //     : analogInput * forceMultiplier * Globals.BALL_ACCELERATION_CONSTANT);
        
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