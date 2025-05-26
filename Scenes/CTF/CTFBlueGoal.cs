using BallDuel.Scenes.CTF;
using Godot;

public partial class CTFBlueGoal : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBallWithFlag ball)
        {
            CTFScene currentScene = GetTree().GetCurrentScene() as CTFScene;
            currentScene?.BallTouchedBlueGoal(ball);
        }
    }
}