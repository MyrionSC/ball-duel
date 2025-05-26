using BallDuel.Scenes.CTF;
using Godot;

public partial class CTFBlueGoal : Area2D
{
    public Sprite2D flagSprite;

    public override void _Ready()
    {
        flagSprite = GetNode<Sprite2D>("BlueGoalFlag");
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBallWithFlag ball)
        {
            CTFScene currentScene = GetTree().GetCurrentScene() as CTFScene;
            currentScene?.BallTouchedBlueGoal(ball);
        }
    }
}