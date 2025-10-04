using Godot;
using System;

public class PlayerUsableManagementBehavior : CharacterBehavior
{
    public UsableItem Item { get; private set; } = null;
    public int Energy { get; private set; } = 0;
    public int MaxEnergy { get; private set; } = 0;

    public PlayerUsableManagementBehavior(Player _self) : base(_self)
    {
    }

    public override void _Ready()
    {
        base._Ready();

        Item = null;
    }

    public void SetItem(UsableItem newItem)
    {
        WorldUtilsTriggers.SpawnItem(self.Mount, self.Position, Item, false);

        Item = newItem;
        if (MaxEnergy == 0)
        {
            MaxEnergy = newItem.MaxEnergy;
            Energy = MaxEnergy;
        }
        else
        {
            Energy = (int)( (float)newItem.MaxEnergy * Energy / MaxEnergy + .5f );
            MaxEnergy = newItem.MaxEnergy;
            if (Energy > MaxEnergy) Energy = MaxEnergy;
        }
    }

    public bool UseItem(Player player)
    {
        if (Item != null)
        {
            Item.OnUse(player);
            Item = null;
            return true;
        }
        return false;
    }
}