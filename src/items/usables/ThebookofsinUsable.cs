using Godot;
using System;

[GlobalClass]
public partial class ThebookofsinUsable : UsableItem
{
    [Export] public SpawnPool Pool;
    public override void OnUse(Player player)
    {
        if (usableManager.TryUseEnergy())
        {
            Item item = Pool.Roll<Item>();
            if (item != null && WorldUtilsRoomManager.CurrentRoom != null)
            {
                WorldUtilsRoomManager.CurrentRoom.AddItem(item, player.GlobalPosition + WorldUtilsRandom.RandomInDisc(15));
            }
        }
    }
}