using Godot;
using System;

public static class WorldUtilsTriggers
{
    private static PackedScene bombScene = GD.Load<PackedScene>(WorldUtilsPools.resourcePaths["bomb_obj"]);
    public static void SpawnBomb(Vector2 position, Node mount)
    {
        BombObj bomb = (BombObj)bombScene.Instantiate();
        bomb.InitAndEnterTree(mount, position);
    }

    private static PackedScene itemObjScene = GD.Load<PackedScene>(WorldUtilsPools.resourcePaths["item_obj"]);
    public static ItemObj SpawnItem(Node mount, Vector2 position, Item item, bool withJelly = true)
    {
        ItemObj obj = (ItemObj)itemObjScene.Instantiate();
        obj.item = item;
        obj.InitAndEnterTree(mount, position);
        if (withJelly)
        {
            obj.GetJellyEffect();
        }
        return obj;
    }
}