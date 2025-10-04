using Godot;
using System;
using System.Collections.Generic;

public class PlayerEffectManagementBehavior : CharacterBehavior
{
    public readonly List<EffectItem> activeEffects = new List<EffectItem>();

    public PlayerEffectManagementBehavior(Player _self) : base(_self)
    {
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public void ApplyEffect(EffectItem effectItem)
    {
        effectItem.OnPlayerGet((Player)self);
        activeEffects.Add(effectItem);
    }
}