using Godot;
using System;

public partial class Tear : Projectile
{
    [Export] public float zposVirtual = 10.0f;
    [Export] public Curve dropCurve;
    [Export] public float knockbackForce = 2f;

    public override void _Ready()
    {
        base._Ready();

        statusF["disabled"] = 0;
        anim.Play("fly");
    }

    protected override void HitTarget(Character target)
    {
        if(statusF["disabled"] == 1) return;
        target.GetBehavior<CharacterHPBehavior>(BehaviorType.HP).TakeDamage(
            new DamageData(attacker, (int)statusF["damage"], DamageType.NORMAL, Velocity.Normalized() * knockbackForce)
        );
        statusF["disabled"] = 1;
    }

    protected override void Destroy()
    {
        if(statusF.ContainsKey("destroyed")) return;
        statusF["destroyed"] = 1;
        anim.Play("hit");
        Velocity = Vector2.Zero;
        //Disable();
        anim.AnimationFinished += (StringName animName) => {
            AfterAnimHit();
        };
    }

    private void AfterAnimHit()
    {
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if(statusF["disabled"] == 1) return;

        Vector2 displacement = new Vector2(0, zposVirtual) *
            dropCurve.Sample(statusF["distance_traveled"] / statusF["range"]);
        Position = xyPosition + displacement;
    }
}