using Godot;
using System;

public static class WorldUtilsRng
{
    private static Random rng;

    public static void Init(int seed)
    {
        rng = new Random(seed);
    }

    public static float RandomRange(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }

    public static int RandomiRange(int min, int max)
    {
        return rng.Next(min, max);
    }

    public static bool Chance(float probability)
    {
        return rng.NextDouble() < probability;
    }
}

public static class WorldUtilsRandom
{
    private static Random rng;

    public static void Init(int seed)
    {
        rng = new Random(seed);
    }

    public static float RandomRange(float min, float max)
    {
        return (float)(rng.NextDouble() * (max - min) + min);
    }

    public static int RandomiRange(int min, int max)
    {
        return rng.Next(min, max);
    }

    public static bool Chance(float probability)
    {
        return rng.NextDouble() < probability;
    }
}