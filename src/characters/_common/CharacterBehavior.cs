using Godot;
using System;
using System.Collections.Generic;

public class CharacterBehavior
{
    public Character self;
    protected Dictionary<string, float> statusF => self.statusF;
    protected Dictionary<string, Vector2> statusV => self.statusV;

    public CharacterBehavior(Character _self)
    {
        self = _self;
    }

    public virtual void PlugIn(){

    }

    public virtual void UnPlug(){

    }

    public virtual void _Ready()
    {
        PlugIn();
    }

    public virtual void _Process(double delta)
    {
        
    }
}
