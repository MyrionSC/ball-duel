namespace BallDuel.scripts;

public static class Globals
{
    public static float RespawnTimeSeconds { get; set; } = 3.0f;
    public static bool InputDisabled { get; set; } = false;
    
    public static float BALL_ACCELERATION_CONSTANT = 250;
    public static float BALL_FORCE_MULTIPLIER_CONSTANT = 2;
}