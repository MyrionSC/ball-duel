using System;
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
            var old = PlayerBall.FORCE_MULTIPLIER_CONSTANT;
            PlayerBall.FORCE_MULTIPLIER_CONSTANT = float.Parse(text);
            Console.WriteLine(
                $"PlayerBall.FORCE_MULTIPLIER_CONSTANT set from {old} to {PlayerBall.FORCE_MULTIPLIER_CONSTANT}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}