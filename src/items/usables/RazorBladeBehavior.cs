using Godot;
using System;

public class RazorBladeBehavior : OneRoomBehavior
{
    private int count = 0;
    public RazorBladeBehavior(Character _self) : base(_self)
    {
    }

    public override void PlugIn()
    {
        base.PlugIn();
        count++;
        statusF["damage"] += 1.2f;
    }

    public override void UnPlug()
    {
        base.UnPlug();
        statusF["damage"] -= 1.2f * count;
        count = 0;
    }
}