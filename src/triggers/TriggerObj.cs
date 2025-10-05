//TriggerObj
//->ItemObj
//->InteractableObj
//->BombObj

using Godot;
using System;

public abstract partial class TriggerObj : Area2D
{
    protected Node mount;

    public void InitAndEnterTree(Node _mount, Vector2 _position)
    {
        mount = _mount;
        mount.AddChild(this);
        GlobalPosition = _position;
    }

    public override void _Ready()
    {
        base._Ready();
        Monitoring = false;
        CollisionLayer = 4;
        CollisionMask = 0;
    }
}