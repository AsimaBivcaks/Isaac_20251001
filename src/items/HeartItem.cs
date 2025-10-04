using Godot;
using System;

[GlobalClass]
public partial class HeartItem : Item
{
    [Export] public int healAmount = 2;

    public override void OnPlayerGet(Player player)
    {
        var hpBehavior = player.GetBehavior<PlayerHPBehavior>(BehaviorType.HP);
        if (hpBehavior != null)
        {
            hpBehavior.Heal(healAmount);
        }
    }

    public override bool IsPickable(Player player)
    {
        var hpBehavior = player.GetBehavior<PlayerHPBehavior>(BehaviorType.HP);
        return hpBehavior != null && hpBehavior.CanHeal();
    }
}