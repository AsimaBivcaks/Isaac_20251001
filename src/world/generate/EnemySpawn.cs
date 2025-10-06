using Godot;
using System;

public partial class EnemySpawn : Spawn
{
    public override void Roll(Room room)
    {
        var scene = Pool.Roll<PackedScene>();
        if (scene != null)
        {
            Character enemy = (Character)scene.Instantiate();
            room.AddCharacter(enemy);
            enemy.GlobalPosition = GlobalPosition;
            GD.Print("room pos", room.GridPosition);
        }
    }
}
