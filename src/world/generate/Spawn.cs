using Godot;
using System;

public abstract partial class Spawn : Marker2D
{
    [Export] public SpawnPool Pool;

    public abstract void Roll(Room room);
}