using Godot;

namespace BallDuel.Scenes.Start;

public partial class AirHockeySelect : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        GetTree().ChangeSceneToFile("res://Scenes/AirHockey/AirHockeyScene.tscn");
    }
}