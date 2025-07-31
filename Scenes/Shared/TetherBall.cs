using System;
using Godot;

namespace BallDuel.Scenes.Shared;

public partial class TetherBall : RigidBody2D
{
    private Vector2 _originalPosition = Vector2.Zero;
    [Export] public Vector2 TetherPosition = Vector2.Zero;
    [Export] public Vector2 StartVelocity = Vector2.Zero;
    private Line2D _line;
    private bool _reset = false;

    public override void _Ready()
    {
        base._Ready();

        if (_originalPosition == Vector2.Zero) _originalPosition = Position;
        if (TetherPosition == Vector2.Zero) TetherPosition = _originalPosition;

        if (GetName().ToString().Contains("Black"))
        {
            var sprite = GetNode<Sprite2D>("Sprite2D");
            sprite.Texture = GD.Load<Texture2D>("res://assets/black_ball.png");
        }

        _line = new Line2D();
        var r = new Random();
        _line.DefaultColor = Color.Color8((byte)(r.NextInt64() % 256), (byte)(r.NextInt64() % 256),
            (byte)(r.NextInt64() % 256));
        _line.Width = 1.5f;
        var parentScene = GetParent<Node2D>();
        parentScene.CallDeferred("add_child", _line);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        if (_reset)
        {
            Position = _originalPosition;
            LinearVelocity = StartVelocity;
            AngularVelocity = 0;
            ConstantForce = Vector2.Zero;
            _line.ClearPoints();
            _reset = false;
            return;
        }

        // try to return to original position
        var newVel = TetherPosition - GetPosition();
        if (!(newVel.Length() > 1f)) return;

        var isBlackBall = GetName().ToString().Contains("Black");
        var forceMultiplier = isBlackBall ? 50f : 5f;

        ApplyForce(newVel * forceMultiplier);
        _line.ClearPoints();
        _line.AddPoint(TetherPosition);
        _line.AddPoint(GetPosition());
    }
    
    public void ResetToStart()
    {
        _reset = true;
    }
}