using Godot;
using System;

public partial class Spike : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        BodyEntered += (Node2D body) =>
        {
            if (body is Character character)
            {
                var hp = character.GetBehavior<CharacterHPBehavior>(BehaviorType.HP);
                if (hp != null)
                {
                    DamageData damage = new DamageData(null, 1, DamageType.NORMAL, Vector2.Zero);
                    hp.TakeDamage(damage);
                }
            }
        };
    }
}