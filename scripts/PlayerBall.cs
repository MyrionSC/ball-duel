using System;
using BallDuel.scripts;
using Godot;

public partial class PlayerBall : RigidBody2D
{
    private bool _resetState = false;
    private Vector2 _newPosition;
    
    public Vector2 OriginalPosition = Vector2.Zero;
    public bool IsRespawning = false;
    private Line2D _line;
    public bool ShouldDrawLine = false;
    public Sprite2D OntopSprite;
    
    public float Accelaration = Globals.BALL_ACCELERATION_CONSTANT;

    [Export] public int ControllerId { get; set; } = 0; // Default to first controller

    public override void _Ready()
    {
        base._Ready();
        OriginalPosition = Position;
        OntopSprite = GetNode<Sprite2D>("OntopSprite"); // is null if not found

        _line = new Line2D();
        var r = new Random();
        _line.DefaultColor = Color.Color8((byte)(r.NextInt64() % 256), (byte)(r.NextInt64() % 256),
            (byte)(r.NextInt64() % 256));
        _line.Width = 1.5f;
        var scene = GetParent<Node2D>();
        scene.CallDeferred("add_child", _line);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);

        if (_resetState)
        {
            _resetState = false;
            Position = _newPosition;
            LinearVelocity = Vector2.Zero;
            return;
        }

        Vector2 analogInput = new Vector2(
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftX),
            GetClampedJoyAxis(ControllerId, JoyAxis.LeftY)
        );
        float forceMultiplier = Input.IsActionPressed($"device_{ControllerId}_trigger_right")
            ? Globals.BALL_FORCE_MULTIPLIER_CONSTANT
            : 1f;

        var forceVector = analogInput * forceMultiplier * Accelaration;

        if (ShouldDrawLine)
        {
            _line.ClearPoints();
            _line.AddPoint(GetPosition());
            _line.AddPoint(GetPosition() + forceVector);
        }
        
        ApplyForce(Globals.InputDisabled
            ? Vector2.Zero
            : forceVector);
    }

    private float GetClampedJoyAxis(int controllerId, JoyAxis axis)
    {
        float clampedJoyAxis = Input.GetJoyAxis(controllerId, axis);
        return Math.Abs(clampedJoyAxis) > 0.1f ? clampedJoyAxis : 0;
    }

    // Useful method to check if the controller is connected
    public bool IsControllerConnected()
    {
        return Input.IsJoyKnown(ControllerId);
    }

    public void ResetPosition()
    {
        MoveBody(OriginalPosition);
    }

    public void MoveBody(Vector2 targetPosition)
    {
        _resetState = true;
        _newPosition = targetPosition;
    }

    public string GetColorName()
    {
        switch (ControllerId)
        {
            case 0:
                return "Blue";
            case 1:
                return "Red";
            case 2:
                return "Green";
            case 3:
                return "Yellow";
            default:
                throw new Exception($"controller id {ControllerId} has no defined color");
        }
    }
}