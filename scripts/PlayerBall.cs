using System;
using BallDuel.scripts;
using Godot;

public partial class PlayerBall : RigidBody2D
{
    public static float ACCELERATION_CONSTANT = 250;
    private bool _resetState = false;
    public Vector2 OriginalPosition = Vector2.Zero;
    private Vector2 _newPosition;
    public static float FORCE_MULTIPLIER_CONSTANT = 2;
    public bool IsRespawning = false;
    
    [Export] public int ControllerId { get; set; } = 0; // Default to first controller

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        if (_resetState)
        {
            _resetState = false;
            Position = _newPosition;
            LinearVelocity = Vector2.Zero;
            return;
        }

        Vector2 analogInput = new Vector2(
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftX),
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftY)
        );
        float forceMultiplier = Input.IsActionPressed($"device_{ControllerId}_trigger_right") ? FORCE_MULTIPLIER_CONSTANT : 1f;
        
        ApplyForce(Globals.InputDisabled
            ? Vector2.Zero
            : analogInput * forceMultiplier * ACCELERATION_CONSTANT);
    }

    private float GetClampedJoyAxis(int controllerId, JoyAxis axis)
    {
        float clampedJoyAxis = Input.GetJoyAxis(controllerId, axis);
        return Math.Abs(clampedJoyAxis) > 0.1f ? clampedJoyAxis : 0;
    }

    // Useful method to check if the controller is connected
    public bool IsControllerConnected()
    {
        return Input.IsJoyKnown(ControllerId);
    }

    public void ResetPosition()
    {
        MoveBody(OriginalPosition);
    }

    public void MoveBody(Vector2 targetPosition)
    {
        _resetState = true;
        _newPosition = targetPosition;
    }
}