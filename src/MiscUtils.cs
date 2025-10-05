using Godot;
using System;

public static class MiscUtils
{
    public static Vector2 GetDirectionVector(float angle)
    {
        return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
    }

    public static Vector2 ComplexMultiply(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X - a.Y * b.Y, a.X * b.Y + a.Y * b.X);
    }

    public static Vector2 Conjugate(Vector2 a)
    {
        return new Vector2(a.X, -a.Y);
    }
}