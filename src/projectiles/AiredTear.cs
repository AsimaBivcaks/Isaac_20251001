using Godot;
using System;

//MUST BE EMITTED BY FACTORY: RAWDIR
//NOT USED BY THE PLAYER
public partial class AiredTear : Projectile
{
    [Export] public float zposVirtual = 120.0f;
    [Export] public Curve dropCurve;
    [Export] public float knockbackForce = 2f;
    //[Export] public float initialVelocity = 100f;

    private Vector2 targetPos;

    public override void _Ready()
    {
        base._Ready();

        anim.Play("fly");

        targetPos = Velocity;
        Disable();
        Velocity = (targetPos - GlobalPosition).Normalized() * SpeedRefValue;
        statusF["range"] = (targetPos - GlobalPosition).Length();
        zposVirtual = Math.Min(zposVirtual, statusF["range"] * .1f);
    }

    protected override void HitTarget(Character target)
    {
        if(statusF["disabled"] > .5f) return;
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

        //if(statusF["disabled"] > .5f) return;
        
        if(statusF["disabled"] > .5f)
        {
            XYPosition += Velocity * (float)delta;
            statusF["distance_traveled"] += (Velocity * (float)delta).Length();
        }
        float ratio = statusF["distance_traveled"] / statusF["range"];
        ZPosition = -zposVirtual * (1 - dropCurve.Sample(ratio));
        Position = XYPosition + new Vector2(0, ZPosition);

        if(ratio > 0.9f && statusF["disabled"] > .5f)
        {
            Enable();
        }
    }
}