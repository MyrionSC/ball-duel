using System;
using BallDuel.Scenes.TetherBallz;
using Godot;

public partial class TetherBall : RigidBody2D
{
    public Vector2 OriginalPosition;
    private Line2D _line;

    public override void _Ready()
    {
        base._Ready();
        OriginalPosition = Position;

        _line = new Line2D();
        var r = new Random();
        _line.DefaultColor = Color.Color8((byte)(r.NextInt64() % 256), (byte)(r.NextInt64() % 256),
            (byte)(r.NextInt64() % 256));
        _line.Width = 1.5f;
        var TetherBallzScene = GetParent<TetherBallzScene>();
        TetherBallzScene.CallDeferred("add_child", _line);
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