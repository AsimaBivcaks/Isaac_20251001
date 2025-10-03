using Godot;
using System;

public class CommonEnemyHPBehavior : CharacterHPBehavior
{
    public CommonEnemyHPBehavior(Character _self, int maxHP, Callable? deathCallback=null) : base(_self, maxHP, deathCallback)
    {
    }

    protected override bool ProcessDamage(DamageData damageData)
    {
        HP -= damageData.damageAmount;

        //TEMP
        GD.Print($"Enemy took {damageData.damageAmount} damage, current HP: {HP}/{MaxHP}");

        // Apply knockback
        statusV["inertia"] += damageData.knockbackVector;
        return damageData.damageAmount > 0;
    }
}