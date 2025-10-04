using Godot;
using System;
using System.Collections.Generic;

public class CharacterBehavior
{
    public Character self { get; private set; }

    //BlackBoards
    protected Dictionary<string, float> statusF => self.statusF;
    protected Dictionary<string, Vector2> statusV => self.statusV;

    public CharacterBehavior(Character _self)
    {
        self = _self;
    }

    //Called when added to character
    //Sometimes can be used as state enter func for state machines
    public virtual void PlugIn(){

    }

    //Called when removed from character
    //Sometimes can be used as state exit func for state machines
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
