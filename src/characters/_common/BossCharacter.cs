using Godot;
using System;
using System.Collections.Generic;

public abstract partial class BossCharacter : Character
{
    public override void _Ready()
    {
        base._Ready();
        
        var currentBosses = WorldUtilsBlackboard.Get<List<Character>>("current_bosses");
        currentBosses.Add(this);
    }
}