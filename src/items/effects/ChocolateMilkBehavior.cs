using Godot;
using System;

//Basically a copy of PlayerEmitBehavior
//Only for the player
public class ChocolateMilkBehavior : CharacterBehavior
{
    
    private double emitCDRefValue;
    private double emitCDTimer = 0;
    
    private double forceAccumulated = 0;

    public ChocolateMilkBehavior(Player _self, double _emitCDRefValue) : base(_self)
    {
        emitCDRefValue = _emitCDRefValue;
    }

    public override void _Ready()
    {
        base._Ready();
        //emitBehavior = new EmitBehavior();
        
        statusF["emitted_this_frame"] = 0;
    }

    public override void PlugIn()
    {
        base.PlugIn();
        forceAccumulated = 0;
        lastFramePressed = false;
    }

    private bool lastEmitHDirLeft = false;
    private bool lastEmitVDirUp = false;
    private bool lastFramePressed = false;
    private Vector2 recDir = Vector2.Zero;
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (emitCDTimer >= 0) emitCDTimer -= delta;
        
        if(self.statusF["pause"] > .5f) return;
        
        if(Input.IsActionJustPressed("e_left"))
            lastEmitHDirLeft = true;
        else if(Input.IsActionJustPressed("e_right"))
            lastEmitHDirLeft = false;
        if(Input.IsActionJustPressed("e_up"))
            lastEmitVDirUp = true;
        else if(Input.IsActionJustPressed("e_down"))
            lastEmitVDirUp = false;
        
        Vector2 dir = Vector2.Zero;
        if (Input.IsActionPressed("e_up")){
            if (Input.IsActionPressed("e_down") && !lastEmitVDirUp)
                dir.Y = 1;
            else
                dir.Y = -1;
        } else if (Input.IsActionPressed("e_down")){
            dir.Y = 1;
        } else if (Input.IsActionPressed("e_left")){
            if (Input.IsActionPressed("e_right") && !lastEmitHDirLeft)
                dir.X = 1;
            else
                dir.X = -1;
        } else if (Input.IsActionPressed("e_right")){
            dir.X = 1;
        } else {
            statusF["emitted_this_frame"] = 0;
            statusV["forced_facing"] = dir;
            if (lastFramePressed)
            {
                statusF["emitted_this_frame"] = 1;
                emitCDTimer = emitCDRefValue / statusF["fire_rate"];
                float damage = statusF["damage"];
                forceAccumulated = Mathf.Min(1*forceAccumulated , 2) * (2.5)/2;
                statusF["damage"] += (float)forceAccumulated - .833333f;
                ((Player)self).projectileFactory.Emit(self, recDir);
                forceAccumulated = 0;
                statusF["damage"] = damage;
            }
            lastFramePressed = false;
            return;
        }
        statusV["forced_facing"] = dir;
        recDir = dir;

        lastFramePressed = true;
        forceAccumulated += delta;
    }
}
