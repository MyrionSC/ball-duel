using System;
using System.Collections.Generic;
using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.ExplodyBall;

public partial class ExplodyBallScene : BaseScene
{
    private ExplodyBall _explodyBall;
    private readonly Random _random = new();
    private const float CountDownTimeConstant = 7f;
    private double _countDown = CountDownTimeConstant;
    private Sprite2D _dangerSprite;

    public override void _Ready()
    {
        base._Ready();

        _explodyBall = GetNode<ExplodyBall>("ExplodyBall");
        _dangerSprite = GetNode<Sprite2D>("ExplodyBall/DangerSprite2D");

        var randomAngle1 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        var randomAngle2 = (float)((_random.NextDouble() - 0.5) * Math.PI * 2);
        const float speed = 50f;
        _explodyBall.LinearVelocity = new Vector2(randomAngle1 * speed, randomAngle2 * speed);

        BlockingMessageController.Init(this);

        CountdownController.Init(this);
        CountdownController.StartCountdown();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (Globals.InputDisabled) return;

        Console.WriteLine($"== {_countDown -= delta} {(int)_countDown}");

        var countdownLabel = GetNode<RichTextLabel>("ExplodyBall/Countdown");
        var old = int.Parse(countdownLabel.Text);
        if ((int)_countDown != old)
            countdownLabel.Text = ((int)_countDown).ToString();

        if ((int)_countDown == 0)
        {
            var playersToEliminate = GetPlayerBalls()
                .Where(player => _explodyBall.IsPlayerInDangerZone(player))
                .ToList();
            foreach (var player in playersToEliminate)
            {
                player.MoveBody(new Vector2(-100000, 0));
                GetTree().CreateTimer(0.1).Timeout += CheckForWin;
            }

            _dangerSprite.Scale = new Vector2(_dangerSprite.Scale.X + 0.25f, _dangerSprite.Scale.Y + 0.25f);

            _countDown = CountDownTimeConstant;
        }
    }

    public override void ResetScene()
    {
        base.ResetScene();
        _explodyBall.Reset();
    }

    private void StartNextRound()
    {
        ResetPositions();
        _dangerSprite.Scale = new Vector2(1, 1);
        CountdownController.StartCountdown();
        _explodyBall.Reset();
    }

    private void CheckForWin()
    {
        Console.WriteLine("Checking for win...");

        // TODO: Check for score win

        List<PlayerBall> remainingPlayerList =
            playerBallList.Where(b => b.IsControllerConnected() && Math.Abs(b.Position.X) < 50000).ToList();

        if (remainingPlayerList.Count == 1)
        {
            PlayerBall remainingPlayer = remainingPlayerList[0];
            var scoreLabel = GetNode<RichTextLabel>("Player" + (remainingPlayer.ControllerId + 1) + "Score");
            var oldScore = int.Parse(scoreLabel.Text);
            Console.WriteLine(
                $"player {remainingPlayer.ControllerId} wins, old score: {oldScore} new score: {oldScore + 1}");
            scoreLabel.Text = (oldScore + 1).ToString();
            StartNextRound();
        }
        else if (remainingPlayerList.Count == 0)
        {
            Console.WriteLine("draw");
            StartNextRound();
        }
        else
        {
            Console.WriteLine(remainingPlayerList.Count + " players left");
        }
    }

    private IEnumerable<PlayerBall> GetPlayerBalls()
    {
        return GetChildren().OfType<PlayerBall>().Where(node => node.Name.ToString().StartsWith("PlayerBall"));
    }
}