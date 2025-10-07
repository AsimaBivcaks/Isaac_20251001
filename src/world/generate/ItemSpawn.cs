using Godot;
using System;

public partial class ItemSpawn : Spawn
{
    public override void Roll(Room room)
    {
        Item item = Pool.Roll<Item>();
        if (item != null)
        {
            if(item is EffectItem || item is UsableItem)
                room.AddItemBase(item, GlobalPosition);
            else
                room.AddItem(item, GlobalPosition);
        }
    }
}
