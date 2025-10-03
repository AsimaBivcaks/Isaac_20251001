using Godot;
using System;

public class PlayerEmitBehavior : CharacterBehavior
{
    
    private double emitCDRefValue;
    private double emitCDTimer = 0;

    public PlayerEmitBehavior(Player _self, double _emitCDRefValue) : base(_self)
    {
        emitCDRefValue = _emitCDRefValue;
    }

    public override void _Ready()
    {
        base._Ready();
        //emitBehavior = new EmitBehavior();
        
        statusF["emitted_this_frame"] = 0;
    }

    private bool lastEmitHDirLeft = false;
    private bool lastEmitVDirUp = false;
    public override void _Process(double delta)
    {
        base._Process(delta);
        
        if (emitCDTimer >= 0) emitCDTimer -= delta;
        
        if(self.statusF["pause"] > .5f) return;
        
        //when both directions are pressed, use the last pressed direction,
        //vertical inputs over horizontal ones.
        //this is not exactly how isaac works
        //the original game's response to simultaneous inputs can be weird and inconsistent
        //  in the original game, press: ←&→, then ←&→&↓, then ←&→
        //  then u will get ↑ shot, wtf?
        //  maybe some kind of bitmask is used
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
            return;
        }
        statusV["forced_facing"] = dir;

        if(emitCDTimer > 0){
            statusF["emitted_this_frame"] = 0;
            return;
        }

        statusF["emitted_this_frame"] = 1;
        emitCDTimer = emitCDRefValue / statusF["fire_rate"];
        ((Player)self).projectileFactory.Emit(self, dir);
    }
}
