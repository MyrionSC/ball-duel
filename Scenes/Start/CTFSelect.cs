using Godot;

namespace BallDuel.Scenes.Start;

public partial class CTFSelect : Area2D
{
    public void OnBodyEntered(Node2D body)
    {
        var newScene = ResourceLoader.Load<PackedScene>("res://Scenes/CTF/CTFScene.tscn");
        GetTree().ChangeSceneToPacked(newScene);
    }
}