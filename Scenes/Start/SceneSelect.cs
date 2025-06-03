using System;
using Godot;

namespace BallDuel.Scenes.Start;

public partial class SceneSelect : Area2D
{
    [Export] public string resString;  // eg: res://Scenes/MinionSwarm/MinionSwarmScene.tscn
    
    public void OnBodyEntered(Node2D body)
    {
        if (resString == null) throw new Exception("resString is null");
        var newScene = ResourceLoader.Load<PackedScene>(resString);
        GetTree().ChangeSceneToPacked(newScene);
    }
}