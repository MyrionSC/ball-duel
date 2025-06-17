using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.Shared;

public class BlockingMessageController
{
    private static Label _label;
    private static Node2D _scene;

    public static void Init(Node2D scene)
    {
        _scene = scene;
        _label = scene.GetNode<Label>("BlockingMessageLabel");
    }
    
    public static void HideBlockingMessage()
    {
        _label.Visible = false;
    }

    public static void ShowBlockingMessage(string message)
    {
        _label.Text = message;
        _label.Visible = true;
    }
}