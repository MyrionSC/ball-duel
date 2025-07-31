using System;
using Godot;

public partial class TetherBall : RigidBody2D
{
    [Export] public Vector2 OriginalPosition = Vector2.Zero;
    private Line2D _line;

    public override void _Ready()
    {
        base._Ready();
        
        if (OriginalPosition == Vector2.Zero) 
            OriginalPosition = Position;
        
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

        // try to return to original position
        var newVel = OriginalPosition - GetPosition();

        if (newVel.Length() > 1f)
        {
            ApplyForce(newVel * 5);
            _line.ClearPoints();
            _line.AddPoint(OriginalPosition);
            _line.AddPoint(GetPosition());
        }
    }
}