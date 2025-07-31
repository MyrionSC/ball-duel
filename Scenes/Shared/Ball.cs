using Godot;

namespace BallDuel.Scenes.Shared;

public partial class Ball : RigidBody2D
{
    private Vector2 _originalPosition = Vector2.Zero;
    [Export] public Vector2 StartVelocity = Vector2.Zero;
    private bool _reset = false;

    public override void _Ready()
    {
        base._Ready();
        if (_originalPosition == Vector2.Zero) _originalPosition = Position;
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
            _reset = false;
        }
    }

    public void ResetToStart()
    {
        _reset = true;
    }
}