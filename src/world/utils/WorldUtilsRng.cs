using Godot;
using System;

//specially for world generation and other non-deterministic (but consistent) things
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

    public static float Randomf()
    {
        return (float)rng.NextDouble();
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

    public static Vector2 RandomInDisc(float r)
    {
        double angle = rng.NextDouble() * Math.PI * 2;
        double R = Math.Sqrt(rng.NextDouble()) * r;
        return new Vector2((float)(Mathf.Cos(angle) * R), (float)(Mathf.Sin(angle) * R));
    }
}