using Godot;

namespace BallDuel.Scenes.Shared;

public class BlockingMessageController
{
    private static Label _label;
    private static Node2D _scene;

    public static void Init(Node2D scene)
    {
        _scene = scene;
        // _label = scene.GetNode<Label>("BlockingMessageLabel");

        _label = new Label();
        _label.Text = "blocking text";
        _label.Visible = false;
        _label.AddThemeFontSizeOverride("font_size", 100);
        _label.AddThemeColorOverride("font_color", Colors.Black);
        
        _label.ZIndex = 200;
        _label.Size = new Vector2(1400, 800);
        _label.Position = new Vector2(-700, -400); // Adjust these values based on your needs
        _label.HorizontalAlignment = HorizontalAlignment.Center;
        _label.VerticalAlignment = VerticalAlignment.Center;
        _scene.AddChild(_label);
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