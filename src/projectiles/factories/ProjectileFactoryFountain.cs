using Godot;
using System;

//direction.X: number of projectiles
//direction.Y: phase offset
public class ProjectileFactoryFountain : ProjectileFactory
{
    public PackedScene projectileScene;

    public ProjectileFactoryFountain(PackedScene scene)
    {
        projectileScene = scene;
    }
    
    public override void Emit(Character owner, Vector2 direction)
    {
        int count = (int)(direction.X + .1f);
        float phase = direction.Y;
        Vector2 w1 = MiscUtils.GetDirectionVector(MathF.Tau / count);
        Vector2 w = MiscUtils.GetDirectionVector(phase);
        for (int i = 0; i < count; i++)
        {
            EmitPartial(owner, w);
            w = MiscUtils.ComplexMultiply(w, w1);
        }
    }

    private void EmitPartial(Character owner, Vector2 direction)
    {
        Projectile p = CreateProjectile(owner,
            direction + SolveInertia(owner.Velocity.Normalized(), direction, .3f, .7f),
            projectileScene);
        if(owner.statusF.ContainsKey("damage"))
        {
            p.statusF["damage"] = owner.statusF["damage"];
            p.statusF["range"] = owner.statusF["range"];
            p.statusF["speed"] = owner.statusF["move_speed"];
        }
        owner.Mount.AddChild(p);
    }
}