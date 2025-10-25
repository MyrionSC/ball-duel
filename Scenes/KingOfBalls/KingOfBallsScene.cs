using System.Linq;
using BallDuel.Scenes.Shared;
using BallDuel.scripts;
using Godot;

namespace BallDuel.Scenes.KingOfBalls;

public partial class KingOfBallsScene : BaseScene
{
    public Area2D FreeCrown { get; set; }

    public override void _Ready()
    {
        base._Ready();
        
        FreeCrown  = GetNode<Area2D>("FreeCrown");
        
        BlockingMessageController.Init(this);
        
        Border.CollisionCallback = body =>
        {
            if (body is PlayerBall playerBall)
            {
                if (playerBall.OntopSprite?.Texture != null)
                {
                    playerBall.OntopSprite.Texture = null;
                    playerBall.Accelaration = Globals.BALL_ACCELERATION_CONSTANT;
                    FreeCrown.Visible = true;
                }
                GetTree().CreateTimer(3.0f).Timeout += playerBall.ResetPosition;
            }
        };
        
        CountdownController.Init(this);
        CountdownController.StartCountdown();
        
        StartScoreCounter();
    }

    private void StartScoreCounter()
    {
        var scoreCounter = new Timer();
        scoreCounter.OneShot = false;
        scoreCounter.WaitTime = 1.0;
        scoreCounter.Timeout += () =>
        {
            var kingBall = playerBallList.SingleOrDefault(x => x.OntopSprite.Texture != null);
            if (kingBall == null) return;
            var scoreLabel = GetNode<RichTextLabel>($"Player{kingBall.ControllerId+1}Score");
            var oldScore = int.Parse(scoreLabel.Text);
            scoreLabel.Text = (oldScore + 1).ToString();
            
            if (oldScore + 1 >= 100)
            {
                Globals.InputDisabled = true;
                BlockingMessageController.ShowBlockingMessage($"{kingBall.GetColorName()} wins!");
            }
        };
        AddChild(scoreCounter);
        scoreCounter.Start();
    }

    public override void ResetScene()
    {
        base.ResetScene();

        Globals.InputDisabled = false;
        
        foreach (var playerBall in playerBallList)
        {
            playerBall.OntopSprite.Texture = null;
            playerBall.Accelaration = Globals.BALL_ACCELERATION_CONSTANT;
        }
        FreeCrown.Visible = true;

        var scoreLabels = GetChildren().OfType<RichTextLabel>()
            .Where(l => l.Name.ToString().Contains("Score"));
        foreach (var scoreLabel in scoreLabels)
            scoreLabel.Text = "0";

        BlockingMessageController.HideBlockingMessage();
    }
    
    public void TouchedFreeCrown(PlayerBall playerBall)
    {
        if (!FreeCrown.Visible) return;
        FreeCrown.Visible = false;
        playerBall.OntopSprite.Texture = GD.Load<Texture2D>("res://assets/crown-white.png");
        playerBall.Accelaration = Globals.BALL_ACCELERATION_CONSTANT * 1.5f;
    }
    
}