using Godot;
using System;
using System.Data;

[GlobalClass]
public abstract partial class UsableItem : Item
{
    [Export] public int MaxEnergy = 1;

    public override void OnPlayerGet(Player player)
    {
        var usableManager = player.GetBehavior<PlayerUsableManagementBehavior>(BehaviorType.PlayerUsableManagement);
        if (usableManager != null)
        {
            usableManager.SetItem(this);
        }
    }

    public override bool IsPickable(Player player)
    {
        return true;
    }

    public abstract void OnUse(Player player);
}