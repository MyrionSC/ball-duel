using BallDuel.Scenes.CTF;
using Godot;

public partial class CTFRedGoal : Area2D
{
    public Sprite2D flagSprite;

    public override void _Ready()
    {
        flagSprite = GetNode<Sprite2D>("RedGoalFlag");
    }
    
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBallWithFlag ball)
        {
            CTFScene currentScene = GetTree().GetCurrentScene() as CTFScene;
            currentScene?.BallTouchedRedGoal(ball);
        }
    }
}