using Godot;
using System;

public static class WorldUtilsTriggers
{
    public static void SpawnBomb(Vector2 position, Node mount)
    {
        PackedScene bombScene = GD.Load<PackedScene>("res://scenes/itemobj/active_bomb.tscn");
        BombObj bomb = (BombObj)bombScene.Instantiate();
        bomb.InitAndEnterTree(mount, position);
    }
}