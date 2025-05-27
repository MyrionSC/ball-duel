using Godot;

namespace BallDuel.Scenes.Start;

public partial class MinionSwarmSelect : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        var newScene = ResourceLoader.Load<PackedScene>("res://Scenes/MinionSwarm/MinionSwarmScene.tscn");
        GetTree().ChangeSceneToPacked(newScene);
    }
}