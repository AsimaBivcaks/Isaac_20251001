using Godot;
using System;

public abstract class CharacterHPBehavior : CharacterBehavior
{
    public int MaxHP { get; protected set; }
    public int HP { get; protected set; }

    public Callable? DeathCallback;

    public CharacterHPBehavior(Player _self, int maxHP, Callable? deathCallback=null) : base(_self)
    {
        this.MaxHP = maxHP;
        DeathCallback = deathCallback;
    }

    public override void _Ready()
    {
        base._Ready();
        HP = MaxHP;
    }

    protected abstract void ProcessDamage(DamageData damageData);

    public void TakeDamage(DamageData damageData)
    {
        ProcessDamage(damageData);
        if (HP <= 0)
        {
            DeathCallback?.Call();
        }
    }
}
