using Godot;
using System;

public partial class Stone : StaticBody2D, IBlastable
{
    public void OnBlast(Vector2 blastOrigin, float damage)
    {
        QueueFree();
    }
}