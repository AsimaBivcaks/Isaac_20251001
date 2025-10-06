using Godot;
using System;

public static class WorldUtilsSpawn
{
    private static PackedScene bombScene = WorldUtilsPools.GetResource<PackedScene>("bomb_obj");
    public static void SpawnBomb(Vector2 position, Node mount)
    {
        BombObj bomb = (BombObj)bombScene.Instantiate();
        bomb.InitAndEnterTree(mount, position);
    }

    //Don't use this outside Room, use Room.AddItem instead
    private static PackedScene itemObjScene = WorldUtilsPools.GetResource<PackedScene>("item_obj");
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

    //Don't use this outside Room, use Room.AddItemBase instead
    private static PackedScene itembaseObjScene = WorldUtilsPools.GetResource<PackedScene>("itembase_obj");
    public static ItemBaseObj SpawnItemBase(Node mount, Vector2 position, Item item)
    {
        ItemBaseObj obj = (ItemBaseObj)itembaseObjScene.Instantiate();
        obj.item = item;
        obj.InitAndEnterTree(mount, position);
        return obj;
    }
    
    //Don't use this outside Room, use Room.AddCharacter instead
    public static Character SpawnEnemy(Node mount, Vector2 position, string name)
    {
        if (!WorldUtilsPools.resourcePaths.ContainsKey(name))
        {
            GD.PrintErr($"[WorldUtilsSpawn] No such enemy name: {name}");
            return null;
        }
        PackedScene scene = GD.Load<PackedScene>(WorldUtilsPools.resourcePaths[name]);
        Character character = (Character)scene.Instantiate();
        mount.AddChild(character);
        character.GlobalPosition = position;
        return character;
    }
}