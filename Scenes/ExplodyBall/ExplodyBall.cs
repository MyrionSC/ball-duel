using Godot;

namespace BallDuel.Scenes.ExplodyBall;

public partial class ExplodyBall : RigidBody2D
{
    private bool _resetState;
    private Vector2 _originalPosition = Vector2.Zero;
    private Sprite2D _dangerSprite;

    public override void _Ready()
    {
        base._Ready();
        _dangerSprite = GetNode<Sprite2D>("DangerSprite2D");
    }

    public void Reset()
    {
        _resetState = true;
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        if (_resetState)
        {
            _resetState = false;
            Position = _originalPosition;
        }
    }

    public bool IsPlayerInDangerZone(Node2D player)
    {
        var dangerArea = _dangerSprite.GetNodeOrNull<Area2D>("Area2D");
        if (dangerArea == null) return false;

        var overlappingBodies = dangerArea.GetOverlappingBodies();
        foreach (var body in overlappingBodies)
        {
            if (body == player)
                return true;
        }

        return false;
    }
}