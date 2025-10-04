using Godot;
using System;

[GlobalClass]
public partial class KeyItem : Item
{
    [Export] public int KeyAmount = 1;

    public override void OnPlayerGet(Player player)
    {
        var keyBehavior = player.GetBehavior<PlayerKeyManagementBehavior>(BehaviorType.PlayerKeyManagement);
        if (keyBehavior != null)
        {
            keyBehavior.AddKey(KeyAmount);
        }
    }

    public override bool IsPickable(Player player)
    {
        return true;
    }
}