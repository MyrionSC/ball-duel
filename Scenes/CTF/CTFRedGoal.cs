using Godot;

public partial class CTFRedGoal : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBall ball)
        {
            var currentScene = GetTree().GetCurrentScene() as CTFScene;
            
            if (ball.ControllerId == 0 || ball.ControllerId == 2)
                currentScene?.BallTouchedRedGoal(ball);
            else
                currentScene?.BallTouchedBlueGoal(ball);
            
        }
    }
}