using Godot;
using System;

public class PlayerHPBehavior : CharacterHPBehavior
{
    [Export] private double invincibilityTime = 1.0f;
    private double invincibilityTimer = 0.0f;

    public PlayerHPBehavior(Player _self, int maxHP, Callable? deathCallback=null) : base(_self, maxHP, deathCallback)
    {
    }

    protected override void ProcessDamage(DamageData damageData)
    {
        if (invincibilityTimer > 0)
            return;
        HP -= damageData.damageAmount;
        //GD.Print($"Player took {damageData.damageAmount} damage, current HP: {HP}/{MaxHP}");
        // Apply knockback
        self.Velocity += damageData.knockbackVector;
        invincibilityTimer = invincibilityTime;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (invincibilityTimer > 0)
            invincibilityTimer -= delta;
    }
}