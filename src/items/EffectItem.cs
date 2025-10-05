using Godot;
using System;

//this class doesn't do much, just to differentiate from other items
public abstract partial class EffectItem : Item
{
    protected PlayerEffectManagementBehavior effectManager;

    public override void OnPlayerGet(Player player) //Don't override this anymore
    {
        //base.OnPlayerGet(player);
        effectManager = player.GetBehavior<PlayerEffectManagementBehavior>(BehaviorType.PlayerEffectManagement);
        OnActive(player);
        effectManager.ApplyEffect(this);
    }
    public virtual void OnActive(Player player)
    {
    }
    public virtual void OnRemove(Player player)
    {
    }

    public override bool IsPickable(Player player)
    {
        return true;
    }
}