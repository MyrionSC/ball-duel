using Godot;

public partial class MinionSwarmRedGoal : Area2D
{
    public override void _Ready()
    {
    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is BallDuel.scripts.PlayerBall ball)
        {
            MinionSwarmScene currentScene = GetTree().GetCurrentScene() as MinionSwarmScene;
            currentScene?.BallTouchedRedGoal(ball);
        }
    }
}