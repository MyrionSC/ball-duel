using Godot;
using System;

public partial class PlayerBall : RigidBody2D
{
    [Export] public int ControllerId { get; set; } = 0; // Default to first controller

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        // Get input for specific controller
        Vector2 analogInput = new Vector2(
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftX),
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftY)
        );

        // Console.WriteLine($"Controller {ControllerId} input: {analogInput}");

        // Apply force based on controller input
        ApplyForce(analogInput * 100);
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
}