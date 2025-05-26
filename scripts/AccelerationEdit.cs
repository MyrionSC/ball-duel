using System;
using BallDuel.Scenes.CTF;
using Godot;

namespace BallDuel.scripts;

public partial class AccelerationEdit : TextEdit
{
    public void CustomTextChanged()
    {
        var text = GetText();
        Console.WriteLine("AccelerationEdit text changed to " + text);
        try
        {
            var old = PlayerBall.ACCELERATION_CONSTANT;
            PlayerBall.ACCELERATION_CONSTANT = float.Parse(text);
            PlayerBallWithFlag.ACCELERATION_CONSTANT = float.Parse(text);
            Console.WriteLine($"PlayerBall.ACCELERATION_CONSTANT set from {old} to {PlayerBall.ACCELERATION_CONSTANT}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}