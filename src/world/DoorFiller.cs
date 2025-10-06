//When there's no room this way, instead of generating a door, we just put a wall called DoorFiller
using Godot;
using System;

public partial class DoorFiller : StaticBody2D
{
    public Vector2I LocalGrid;
    public Vector2I LeadsTo;

    public override void _Ready()
    {
        base._Ready();
        Room.DoorPosCalc(this, LocalGrid, LeadsTo);
    }
}