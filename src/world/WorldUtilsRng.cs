using Godot;
using System;

public static class WorldUtilsRng
{
    private static Random rng = new Random();
    
    public static float RandomRange(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }

    public static int RandomRange(int min, int max)
    {
        return rng.Next(min, max);
    }

    public static bool Chance(float probability)
    {
        return rng.NextDouble() < probability;
    }
}