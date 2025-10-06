using Godot;
using System;

public class CommonEnemyHPBehavior : EnemyHPBehavior
{
    public CommonEnemyHPBehavior(Character _self, int maxHP, Callable? deathCallback=null) : base(_self, maxHP, deathCallback)
    {
    }

    protected override bool ProcessDamage(DamageData damageData)
    {
        HP -= damageData.damageAmount;

        //DEBUG
        GD.Print($"Enemy took {damageData.damageAmount} damage, current HP: {HP}/{MaxHP}");

        // Apply knockback
        statusV["inertia"] += damageData.knockbackVector;
        return damageData.damageAmount > 0;
    }
}