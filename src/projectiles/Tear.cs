using Godot;
using System;

public partial class Tear : Projectile
{
    [Export] public float zposVirtual = 10.0f;
    [Export] public Curve dropCurve;

    public override void _Ready()
    {
        base._Ready();

        statusF["disabled"] = 0;
        anim.Play("fly");
    }

    protected override void HitTarget(Player target)
    {
        if(statusF["disabled"] == 1) return;
        target.GetBehavior<PlayerHPBehavior>(BehaviorType.HP).TakeDamage(
            new DamageData(attacker, (int)statusF["damage"], DamageType.NORMAL, Vector2.Zero)
        );
        statusF["disabled"] = 1;
    }

    protected override void Destroy()
    {
        if(statusF["disabled"] == 1) return;
        statusF["disabled"] = 1;
        anim.Play("hit");
        Disable();
        anim.AnimationFinished += (StringName animName) => {
            QueueFree();
        };
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