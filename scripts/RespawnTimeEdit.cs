using System;
using Godot;

namespace BallDuel.scripts;

public partial class RespawnTimeEdit : TextEdit
{
    public void CustomTextChanged()
    {
        var text = GetText();
        Console.WriteLine("RespawnTimeEdit text changed to " + text);
        try
        {
            Globals.RespawnTimeSeconds = float.Parse(text);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}