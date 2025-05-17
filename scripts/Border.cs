using System;
using Godot;

public partial class Border : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBall ball)
        {
            ball.MoveBody(new Vector2(-100000, 0));
            var versusScene = GetParent() as VersusScene;
            versusScene!.CheckForWin();
        }
    }
}