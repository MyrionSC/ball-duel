using Godot;

namespace BallDuel.Scenes.Start;

public partial class VersusSelect : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        var newScene = ResourceLoader.Load<PackedScene>("res://Scenes/Versus/VersusScene.tscn");
        GetTree().ChangeSceneToPacked(newScene);
    }
}