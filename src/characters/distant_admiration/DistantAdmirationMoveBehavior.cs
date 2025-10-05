using Godot;
using System;

public class DistantAdmirationMoveBehavior : CharacterBehavior
{
    private const float RADIUS = 48f;
    private const float OMEGA = 2f; //rad/s

    private Node2D follow;

    private float theta = 0f;

    public DistantAdmirationMoveBehavior(Character _self, Node2D _follow) : base(_self)
    {
        follow = _follow;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        theta += OMEGA * (float)delta;
        self.GlobalPosition = follow.GlobalPosition + new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * RADIUS;

        self.Visible = true;
    }
}