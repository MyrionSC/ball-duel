using System;
using Godot;

namespace BallDuel.Scenes.TetherVersus;

public partial class TetherVersusBall : RigidBody2D
{
    public Vector2 OriginalPosition;
    private Line2D _line;
    private PlayerBall _playerBall;
    private bool _resetState;
    private Vector2 _newPosition;

    public override void _Ready()
    {
        base._Ready();
        OriginalPosition = Position;

        _line = new Line2D();
        var r = new Random();
        _line.DefaultColor = Color.Color8((byte)(r.NextInt64() % 256), (byte)(r.NextInt64() % 256),
            (byte)(r.NextInt64() % 256));
        _line.Width = 1.5f;
        var parentScene = GetParent<TetherVersusScene>();
        parentScene.CallDeferred("add_child", _line);
    }

    public void TetherToPlayer(PlayerBall playerBall)
    {
        _playerBall = playerBall;
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

        if (_playerBall == null) return;
        var newVel = _playerBall.GetPosition() - GetPosition();
        if (newVel.Length() > 1f)
        {
            ApplyForce(newVel * 3);
            _line.ClearPoints();
            _line.AddPoint(_playerBall.GetPosition());
            _line.AddPoint(GetPosition());
        }
    }

    public void Reset()
    {
        Freeze = false;
        _resetState = true;
        _newPosition = OriginalPosition;
    }

    public void MoveBody(int x, int y)
    {
        _resetState = true;
        _newPosition = new Vector2(x, y);
    }
}