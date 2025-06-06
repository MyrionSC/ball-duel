using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.Shared;

public class CountdownController
{
    private static int _countdownNumber = 3;
    private static Label _label;
    private static Timer _countdownTimer;
    private static Node2D _scene;

    public static void Init(Node2D scene)
    {
        _scene = scene;
        _label = scene.GetNode<Label>("CountdownLabel");

        _countdownTimer = new Timer();
        scene.AddChild(_countdownTimer);
        _countdownTimer.WaitTime = 1.0f; // 1 second intervals
        _countdownTimer.OneShot = false; // Will fire repeatedly
        _countdownTimer.Timeout += OnCountdownTimerOnTimeout;
    }

    private static void OnCountdownTimerOnTimeout()
    {
        _countdownNumber--;

        if (_countdownNumber <= 0)
        {
            _countdownTimer.Stop();
            Globals.InputDisabled = false;
            _label.Visible = false;
            _countdownNumber = 3;
        }

        var labelText = _countdownNumber.ToString();
        _label.Text = labelText;
    }

    public static void StartCountdown()
    {
        Globals.InputDisabled = true;
        _label.Visible = true;
        _countdownTimer.Start();
    }
}