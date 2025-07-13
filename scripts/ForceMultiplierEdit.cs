using System;
using BallDuel.Scenes.CTF;
using Godot;

namespace BallDuel.scripts;

public partial class ForceMultiplierEdit : TextEdit
{
    public void ForceMultiplierTextChanged()
    {
        var text = GetText();
        Console.WriteLine("ForceMultiplierEdit text changed to " + text);
        try
        {
            var old = Globals.BALL_FORCE_MULTIPLIER_CONSTANT;
            Globals.BALL_FORCE_MULTIPLIER_CONSTANT = float.Parse(text);
            Console.WriteLine(
                $"PlayerBall.FORCE_MULTIPLIER_CONSTANT set from {old} to {Globals.BALL_FORCE_MULTIPLIER_CONSTANT}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}