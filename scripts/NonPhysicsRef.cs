using System;
using Godot;

namespace BallDuel.scripts;

public partial class NonPhysicsRef : Area2D
{
    [Export] public double MoveSpeed = 50;
    [Export] public Vector2 Direction = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 analogInput = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (analogInput.Length() > 0.1f) // Small deadzone
        {
            Direction = analogInput.Normalized();
        }
        else
        {
            Direction = Vector2.Zero;
        }

        Position = Position with
        {
            X = (float)(Position.X + MoveSpeed * delta * Direction.X),
            Y = (float)(Position.Y + MoveSpeed * delta * Direction.Y)
        };
    }
}