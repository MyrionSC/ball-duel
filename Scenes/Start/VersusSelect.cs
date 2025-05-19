using Godot;

namespace BallDuel.Scenes.Start;

public partial class VersusSelect : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        var sceneTree = GetTree();
        var newScene = ResourceLoader.Load<PackedScene>("res://Scenes/Versus/VersusScene.tscn");
        sceneTree.ChangeSceneToPacked(newScene);
    }
}