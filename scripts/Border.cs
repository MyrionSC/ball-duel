using BallDuel.Scenes.CTF;
using Godot;

public partial class Border : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBall ball)
        {
            ball.MoveBody(new Vector2(-100000, 0));
        }
        if (body is PlayerBallWithFlag ballWithFlag)
        {
            ballWithFlag.MoveBody(new Vector2(-100000, 0));
            ballWithFlag.SetHasFlag(false);
            var currentScene = GetTree().GetCurrentScene() as CTFScene;
            if (ballWithFlag.isBlue())
            {
                currentScene.RedGoal.flagSprite.SetVisible(true);
            }
            else
            {
                currentScene.BlueGoal.flagSprite.SetVisible(true);
            }
            GetTree().CreateTimer(3).Timeout += () =>
            {
                ballWithFlag.Reset();
            };
        }
    }
}