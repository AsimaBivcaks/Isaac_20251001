using Godot;
using System;

public class KnightHPBehavior : CharacterHPBehavior
{
    public KnightHPBehavior(Character _self, int maxHP, Callable? deathCallback=null) : base(_self, maxHP, deathCallback)
    {
    }

    protected override bool ProcessDamage(DamageData damageData)
    {
        int damageAmount = damageData.damageAmount;
        if(damageData.damageType == DamageType.NORMAL)
        {
            //front shield
            if(damageData.knockbackVector.Normalized().Dot(statusV["facing"]) < -.6f)
                damageAmount = 0;
        }
        HP -= damageAmount;

        //TEMP
        GD.Print($"Knight took {damageAmount} damage, current HP: {HP}/{MaxHP}");

        // Apply knockback
        statusV["inertia"] += damageData.knockbackVector;
        return damageAmount > 0;
    }
}