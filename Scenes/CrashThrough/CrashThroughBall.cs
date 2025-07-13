using System;
using BallDuel.Scenes.CrashThrough;
using Godot;

public partial class CrashThroughBall : RigidBody2D
{
    public Vector2 OriginalPosition = Vector2.Zero;
    private Line2D _line = new();

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        // try to return to original position
        var newVel = OriginalPosition - GetPosition();
        Console.WriteLine(newVel);

        if (newVel.Length() > 1f)
        {
            ApplyForce(newVel * 5);
            _line.ClearPoints();
            _line.AddPoint(OriginalPosition);
            _line.AddPoint(GetPosition());
        }
    }

    public void InitLine(CrashThroughScene crashThroughScene)
    {
        _line.DefaultColor = Colors.Red;
        _line.Width = 1.5f;
        crashThroughScene.AddChild(_line);
    }
    
}