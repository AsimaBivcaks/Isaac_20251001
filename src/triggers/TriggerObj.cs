using Godot;
using System;

public partial class TriggerObj : Area2D
{
    //#nullable enable
    [Export] public Item item;

    protected Node mount;

    public void InitAndEnterTree(Node _mount, Vector2 _position)
    {
        Position = _position;
        mount = _mount;
        mount.AddChild(this);
    }

    public override void _Ready()
    {
        base._Ready();
        Monitoring = false;
        CollisionLayer = 4;
        CollisionMask = 0;
    }
}