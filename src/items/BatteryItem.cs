using Godot;
using System;

[GlobalClass]
public partial class BatteryItem : Item
{
    [Export] public int energyAmount = 1;

    public override void OnPlayerGet(Player player)
    {
        var usableManager = player.GetBehavior<PlayerUsableManagementBehavior>(BehaviorType.PlayerUsableManagement);
        if (usableManager != null)
        {
            usableManager.FillEnergy(energyAmount);
        }
    }

    public override bool IsPickable(Player player)
    {
        var usableManager = player.GetBehavior<PlayerUsableManagementBehavior>(BehaviorType.PlayerUsableManagement);
        return usableManager != null && usableManager.CanFillEnergy();
    }
}