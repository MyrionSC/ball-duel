using System;
using Godot;

public partial class CrashThroughBall : RigidBody2D
{
    public Vector2 OriginalPosition = Vector2.Zero;

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        
        // try to return to original position

        Console.WriteLine();


        var position = GetPosition();
        var newVel = OriginalPosition - position;
        Console.WriteLine(newVel);
        if (newVel.Length() > 1f)
        {
            ApplyForce(newVel);
        }
        
        
        // ApplyForce(Globals.InputDisabled ? Vector2.Zero : analogInput * forceMultiplier * Globals.BALL_ACCELERATION_CONSTANT);
        
        
    }

}
