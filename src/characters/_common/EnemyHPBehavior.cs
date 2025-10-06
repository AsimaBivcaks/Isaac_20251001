using Godot;
using System;

public abstract class EnemyHPBehavior : CharacterHPBehavior
{
    public SpawnPool DeathSpawnPool { get; set; } = null;

    public EnemyHPBehavior(Character _self, int _maxHP, Callable? deathCallback = null) : base(_self, _maxHP, deathCallback)
    {
    }

    public override void _Ready()
    {
        base._Ready();
        var originalDeathCallback = DeathCallback;
        DeathCallback = Callable.From(() => {
            DeathSpawn();
            if (WorldUtilsRoomManager.CurrentRoom != null)
            {
                WorldUtilsRoomManager.CurrentRoom.OnEnemyDeath();
            }
            originalDeathCallback?.Call();
        });
    }

    private void DeathSpawn()
    {
        if (DeathSpawnPool != null)
        {
            Item item = DeathSpawnPool.Roll<Item>();
            if (item != null && WorldUtilsRoomManager.CurrentRoom != null)
            {
                WorldUtilsRoomManager.CurrentRoom.AddItem(item, self.GlobalPosition + WorldUtilsRandom.RandomInDisc(30));
            }
        }
    }
}