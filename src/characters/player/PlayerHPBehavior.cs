using Godot;
using System;

public class PlayerHPBehavior : CharacterHPBehavior
{
    private AnimationPlayer anim;
    private Tween hitTween;

    [Export] private double invincibilityTime = 1.0f;
    private double invincibilityTimer = 0.0f;

    public PlayerHPBehavior(Player _self, int maxHP, Callable? deathCallback=null) : base(_self, maxHP, deathCallback)
    {
    }

    protected override bool ProcessDamage(DamageData damageData)
    {
        if (invincibilityTimer > 0)
            return false;
        HP -= damageData.damageAmount;
        //GD.Print($"Player took {damageData.damageAmount} damage, current HP: {HP}/{MaxHP}");
        // Apply knockback
        statusV["inertia"] += damageData.knockbackVector;
        invincibilityTimer = invincibilityTime;
        if(damageData.damageAmount > 0){
            if(hitTween != null)
            {
                hitTween.Kill();
            }
            hitTween = self.GetTree().CreateTween();
            hitTween.TweenCallback(Callable.From(() => {
                anim.Play("hit");
            })).SetDelay(REDBLINK_DURATION);
            return true;
        }
        return false;
    }

    public override void _Ready()
    {
        base._Ready();
        
        anim = ((Player)self).anim.HitAnim;
        anim.AnimationFinished += (StringName name) => {
            if (invincibilityTimer <= 0)
                anim.Stop();
            else anim.Play("hit");
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (invincibilityTimer > 0)
            invincibilityTimer -= delta;
    }
}