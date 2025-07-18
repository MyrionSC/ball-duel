using System;
using BallDuel.Scenes.CTF;
using BallDuel.scripts;
using Godot;

public partial class Border : Area2D
{
    public static Action<Node2D> CollisionCallback { get; set; } = null;

    public void OnBodyEntered(Node2D body)
    {
        if (body is PlayerBall ball)
        {
            ball.MoveBody(new Vector2(-100000, 0));
            if (ball.IsRespawning)
            {
                GetTree().CreateTimer(Globals.RespawnTimeSeconds).Timeout += () => { ball.ResetPosition(); };
            }
        }

        if (body is PlayerBallWithFlag ballWithFlag)
        {
            ballWithFlag.MoveBody(new Vector2(-100000, 0));
            var currentScene = GetTree().GetCurrentScene() as CTFScene;
            if (ballWithFlag.HasFlag())
            {
                if (ballWithFlag.IsBlue())
                    currentScene.RedGoal.flagSprite.SetVisible(true);
                else
                    currentScene.BlueGoal.flagSprite.SetVisible(true);
            }

            GetTree().CreateTimer(Globals.RespawnTimeSeconds).Timeout += () => { ballWithFlag.Reset(); };
        }

        CollisionCallback?.Invoke(body);
    }
}