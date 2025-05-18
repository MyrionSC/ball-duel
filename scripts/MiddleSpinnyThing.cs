using Godot;

namespace BallDuel.scripts;

public partial class MiddleSpinnyThing : RigidBody2D
{
    private bool _resetState = false;
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_resetState)
        {
            _resetState = false;
            LinearVelocity = Vector2.Zero;
            Position = Vector2.Zero;
        }
    }
    
    public void Reset()
    {
        _resetState = true;
    }
}