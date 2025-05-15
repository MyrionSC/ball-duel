using Godot;
using System;

public partial class VersusScene : Node2D
{
    public override void _Ready()
    {
        base._Ready();
        Console.WriteLine("VersusScene ready");

        Console.WriteLine("Connected joypads: " + Input.GetConnectedJoypads());

        // var player1 = GetNode<PlayerBall>("PlayerBall1");
        // var player2 = GetNode<PhysicsBall1>("PlayerBall2");
        // player1.ControllerId = 0; // First controller
        // player2.ControllerId = 1; // Second controller
        
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event is InputEventMouseMotion mouseEvent)
        {
            return;
        }

        ;

        // Console.WriteLine(@event.AsText());

        // InputEventJoypadMotion joypadEvent = @event as InputEventJoypadMotion;
        // InputEventJoypadConnection
        // if (@event is InputEventJoypadConnection joypadEvent)
        // {
        //     if (joypadEvent.Connected)
        //         Console.WriteLine($"Controller {joypadEvent.DeviceId} connected");
        //     else
        //         Console.WriteLine($"Controller {joypadEvent.DeviceId} disconnected");
        // }
    }
}