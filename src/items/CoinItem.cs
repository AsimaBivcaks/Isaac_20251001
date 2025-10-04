using Godot;
using System;

[GlobalClass]
public partial class CoinItem : Item
{
    [Export] public int MoneyAmount = 1;

    public override void OnPlayerGet(Player player)
    {
        var moneyBehavior = player.GetBehavior<PlayerMoneyManagementBehavior>(BehaviorType.PlayerMoneyManagement);
        if (moneyBehavior != null)
        {
            moneyBehavior.AddMoney(MoneyAmount);
        }
    }

    public override bool IsPickable(Player player)
    {
        return true;
    }
}