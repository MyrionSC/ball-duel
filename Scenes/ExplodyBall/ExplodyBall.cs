using Godot;

namespace BallDuel.Scenes.ExplodyBall;

public partial class ExplodyBall : RigidBody2D
{
    private bool _resetState;
    private Vector2 _originalPosition = Vector2.Zero;

    public void Reset()
    {
        _resetState = true;
    }
    
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_resetState)
        {
            _resetState = false;
            Position = _originalPosition;
        }
    }
}