using System;
using Godot;

public partial class BallPlayer1 : Area2D
{
    [Export] public double MoveSpeed = 50;
    [Export] public Vector2 Direction = Vector2.Zero;

    public override void _PhysicsProcess(double delta)
    {
        // Get input from both analog stick and d-pad
        Vector2 analogInput = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Console.WriteLine(analogInput);
        
        // If there's analog input, use it
        if (analogInput.Length() > 0.1f) // Small deadzone
        {
            Direction = analogInput.Normalized();
        }
        else
        {
            Direction = Vector2.Zero;
        }

        // If RT is pressed on controller, double direction
        if (Input.IsActionPressed("trigger_right"))
        {
            Direction *= 2;
        }

        Position = Position with
        {
            X = (float)(Position.X + MoveSpeed * delta * Direction.X),
            Y = (float)(Position.Y + MoveSpeed * delta * Direction.Y)
        };
    }
}