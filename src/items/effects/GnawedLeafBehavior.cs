using Godot;
using System;

public class GnawedLeafBehavior : CharacterBehavior
{
    private double stableTimer = 0.0f;

    public GnawedLeafBehavior(Character _self) : base(_self)
    {
    }

    public override void PlugIn()
    {
        base.PlugIn();
        stableTimer = .0f;
    }

    public override void UnPlug()
    {
        base.UnPlug();
        self.statusF["GLB_invincible"] = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        stableTimer += delta;
        if (stableTimer >= 1.0f)
        {
            self.statusF["GLB_invincible"] = 1;
        }

        if (self.statusF["emitted_this_frame"] > .5f || self.statusV["move"].LengthSquared() > .01f)
        {
            stableTimer = 0.0f;
            self.statusF["GLB_invincible"] = 0;
        }
    }
}