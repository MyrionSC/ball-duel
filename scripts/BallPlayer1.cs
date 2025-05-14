using Godot;
using System;

public partial class BallPlayer1 : Area2D
{
    [Export] public double MoveSpeed = 30;
    [Export] public Vector2 Direction = Vector2.Left;
    public long ticks = 0;
    
    public override void _PhysicsProcess(double delta)
    {
        Position = Position with
        {
            X = (float)(Position.X + MoveSpeed * delta * Direction.X),
            Y = (float)(Position.Y + MoveSpeed * delta * Direction.Y)
        };
    }
    
}
