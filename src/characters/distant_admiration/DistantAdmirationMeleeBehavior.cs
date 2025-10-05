using Godot;
using System;

public class DistantAdmirationMeleeBehavior : CharacterBehavior
{
    private Area2D meleeArea;

    private float meleeCD = 1 / 15f;
    private float meleeCDTimer = 0f;

    public DistantAdmirationMeleeBehavior(Character _self, Area2D _meleeArea) : base(_self)
    {
        meleeArea = _meleeArea;
    }

    public override void _Ready()
    {
        base._Ready();
        statusF["no_melee"] = 0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(self.statusF["no_melee"] > .5f) return;
        if(meleeCDTimer <= 0f)
        {
            var bodies = meleeArea.GetOverlappingBodies();
            foreach(var body in bodies)
            {
                if(body is Character character)
                {
                    CharacterHPBehavior hp = character.GetBehavior<CharacterHPBehavior>(BehaviorType.HP);
                    if(hp != null && character != self)
                    {
                        DamageData dmg = new DamageData(self, 5, DamageType.MELEE, Vector2.Zero);
                        hp.TakeDamage(dmg);
                    }
                    meleeCDTimer = meleeCD;
                }
            }
        }
        else meleeCDTimer -= (float)delta;
    }
}