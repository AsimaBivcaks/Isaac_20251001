using Godot;
using System;

public partial class Explosion : Projectile
{
    private bool next_frame = false;

    public override void _Ready()
    {
        base._Ready();

        anim.Play("default");
        anim.AnimationFinished += (StringName animName) => {
            QueueFree();
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(statusF["disabled"] > .5f)
            return;
        if(!next_frame){
            next_frame = true;
        }else{
            ProcessDamage();
            Disable();
        }
    }

    private void ProcessDamage()
    {
        //damage nearby enemies
        var bodies = hitbox.GetOverlappingBodies();
        foreach(var body in bodies){
            if((body is Character target) && ((target.CollisionLayer & targetLayer) != 0)){
                DamageData damage = new DamageData(
                    null,
                    DamageRefValue,
                    DamageType.EXPLOSION,
                    (target.Position - Position).Normalized() * KnockbackRefValue
                );
                target.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(damage);
            }
        }
    }

    protected override void HitTarget(Character target)
    {
    }

    protected override void Destroy() //dont really need this function
    {
    }
}