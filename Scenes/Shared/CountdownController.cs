using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.Shared;

public class CountdownController
{
    private static int _countdownNumber = 3;
    private static Label _label;
    private static Timer _countdownTimer;
    private static Node2D _scene;
    public static bool IsInitted = false;

    public static void Init(Node2D scene)
    {
        _scene = scene;
        _label = scene.GetNode<Label>("CountdownLabel");
        _label.ZIndex = 100;

        _countdownTimer = new Timer();
        scene.AddChild(_countdownTimer);
        _countdownTimer.WaitTime = 1.0f; // 1 second intervals
        _countdownTimer.OneShot = false; // Will fire repeatedly
        _countdownTimer.Timeout += OnCountdownTimerOnTimeout;
        IsInitted = true;
    }

    private static void OnCountdownTimerOnTimeout()
    {
        _countdownNumber--;

        if (_countdownNumber <= 0)
        {
            _countdownTimer.Stop();
            PhysicsServer2D.SetActive(true);
            Globals.InputDisabled = false;
            _label.Visible = false;
            _countdownNumber = 3;
        }

        var labelText = _countdownNumber.ToString();
        _label.Text = labelText;
    }

    public static void StartCountdown()
    {
        if (_scene is null) return;
        // Wait a short while before disabling physics so reset movement have time to take effect
        _scene.GetTree().CreateTimer(0.06).Timeout += () => PhysicsServer2D.SetActive(false);
        Globals.InputDisabled = true;
        _label.Visible = true;
        _countdownTimer.Start();
    }
}