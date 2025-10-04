using Godot;
using System;

//this class doesn't do much, just to differentiate from other items
public abstract partial class EffectItem : Item
{
    //public abstract void OnPlayerGet(Player player);
    public abstract void OnPlayerRemove(Player player);

    public override bool IsPickable(Player player)
    {
        return true;
    }
}