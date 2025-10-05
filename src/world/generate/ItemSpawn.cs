using Godot;
using System;

public partial class ItemSpawn : Spawn
{
    public override void Roll(Room room)
    {
        Item item = Pool.Roll<Item>();
        if (item != null)
        {
            room.AddItem(item, GlobalPosition);
        }
    }
}
