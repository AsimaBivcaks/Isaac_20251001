using Godot;
using System;

public class CommonEnemyMeleeBehavior : CharacterBehavior
{
    Area2D meleeArea;
    bool lastFrameNoMelee = false;

    public CommonEnemyMeleeBehavior(Character _self, Area2D _meleeArea) : base(_self)
    {
        meleeArea = _meleeArea;
    }

    public override void _Ready()
    {
        base._Ready();
        statusF["no_melee"] = 0f;
        
        meleeArea.BodyEntered += (Node2D body) => {
            if(statusF["pause"] > .5f || statusF["no_melee"] > .5f) return;
            if (body is Character target)
            {
                DamageData damage = new DamageData(
                    self, 1, DamageType.MELEE,
                    (target.Position - self.Position).Normalized() * 10f
                );
                target.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(damage);
            }
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(lastFrameNoMelee && statusF["no_melee"] < .5f)
        {
            if(statusF["pause"] > .5f || statusF["no_melee"] > .5f) return;
            foreach(Node2D body in meleeArea.GetOverlappingBodies())
                if (body is Character target)
                {
                    DamageData damage = new DamageData(
                        self, 1, DamageType.MELEE,
                        (target.Position - self.Position).Normalized() * 10f
                    );
                    target.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(damage);
                }
        }
        
        lastFrameNoMelee = statusF["no_melee"] > .5f;
    }
}