using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

public partial class StartScene : BaseScene
{
    public override void _Ready()
    {
        base._Ready();
        Globals.InputDisabled = false;
        PhysicsServer2D.SetActive(true);
    }
}