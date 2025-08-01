using System;
using Godot;

namespace BallDuel.Scenes.Shared;

public partial class TetherBall : RigidBody2D
{
    private Vector2 _originalPosition = Vector2.Zero;
    [Export] public Vector2 TetherPosition = Vector2.Zero;
    [Export] public Vector2 StartVelocity = Vector2.Zero;
    [Export] public TetherBallMode mode = TetherBallMode.Normal;
    private Line2D _line;
    private bool _reset = false;
    private float forceMultiplier = 1f;

    public override void _Ready()
    {
        base._Ready();

        if (_originalPosition == Vector2.Zero) _originalPosition = Position;
        if (TetherPosition == Vector2.Zero) TetherPosition = _originalPosition;
        if (StartVelocity == Vector2.Zero) LinearVelocity = StartVelocity;

        _reset = true;

        if (GetName().ToString().Contains("Black"))
        {
            forceMultiplier = Mass;
            var sprite = GetNode<Sprite2D>("Sprite2D");
            sprite.Texture = GD.Load<Texture2D>("res://assets/black_ball.png");
        }
        
        if (mode == TetherBallMode.Orbit)
        {
            Console.WriteLine(Mass);
            var orbitVelocity = (Position - TetherPosition).Length();
            Console.WriteLine(orbitVelocity);
            StartVelocity = new Vector2(0, orbitVelocity);
            LinearVelocity = StartVelocity;
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

        var newVel = TetherPosition - GetPosition();
        if (!(newVel.Length() > 1f)) return;

        ApplyForce(newVel * forceMultiplier);
        _line.ClearPoints();
        _line.AddPoint(TetherPosition);
        _line.AddPoint(GetPosition());
    }

    public void ResetToStart()
    {
        _reset = true;
    }
    
    public enum TetherBallMode
    {
        Normal, Orbit, OrbitCounter
    }
}
