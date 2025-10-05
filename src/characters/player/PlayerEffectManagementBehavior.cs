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

    public int GetEffectCount(EffectItem effectItem)
    {
        int count = 0;
        foreach(var effect in activeEffects)
        {
            if(effect.GetType() == effectItem.GetType())
            {
                count++;
            }
        }
        return count;
    }

    public EffectItem[] GetEffectsOfType(EffectItem effectItem)
    {
        List<EffectItem> list = new List<EffectItem>();
        foreach(var effect in activeEffects)
        {
            if(effect.GetType() == effectItem.GetType())
            {
                list.Add(effect);
            }
        }
        return list.ToArray();
    }

    public void ApplyEffect(EffectItem effectItem)
    {
        //effectItem.OnPlayerGet((Player)self);
        activeEffects.Add(effectItem);
    }

    public void RemoveEffect(EffectItem effectItem)
    {
        effectItem.OnRemove((Player)self);
        activeEffects.Remove(effectItem);
    }
}