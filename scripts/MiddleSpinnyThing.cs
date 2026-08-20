using Godot;

namespace BallDuel.scripts;

public partial class MiddleSpinnyThing : RigidBody2D
{
    private bool _resetState = false;
    private float _startConstantTorque;

    public override void _Ready()
    {
        _startConstantTorque = ConstantTorque;
        base._Ready();
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_resetState)
        {
            _resetState = false;
            LinearVelocity = Vector2.Zero;
            Position = Vector2.Zero;
            Rotation = 0;
            AngularVelocity = 0;
            ConstantTorque = _startConstantTorque;
        }
    }
    
    public void Reset()
    {
        _resetState = true;
    }
}