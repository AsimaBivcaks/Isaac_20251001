using Godot;
using System;

public class CommonEnemyMeleeBehavior : CharacterBehavior
{
    Area2D meleeArea;

    public CommonEnemyMeleeBehavior(Character _self, Area2D _meleeArea) : base(_self)
    {
        meleeArea = _meleeArea;
    }

    public override void _Ready()
    {
        base._Ready();
        
        meleeArea.BodyEntered += (Node2D body) => {
            if (body is Character target)
            {
                DamageData damage = new DamageData(
                    self, 1, DamageType.COLLISION,
                    (target.Position - self.Position).Normalized() * 10f
                );
                target.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(damage);
            }
        };
    }
}