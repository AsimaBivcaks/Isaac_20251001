using Godot;
using System;
using System.Collections.Generic;

//direction.X: number of projectiles
//direction.Y: phase offset
public class ProjectileFactoryFountain : ProjectileFactoryConcrete
{
    //public PackedScene projectileScene;

    public ProjectileFactoryFountain(PackedScene scene) : base(scene)
    {
    }
    
    public override Projectile[] Emit(Character owner, Vector2 direction)
    {
        int count = (int)(direction.X + .1f);
        float phase = direction.Y;
        Vector2 w1 = MiscUtils.GetDirectionVector(MathF.Tau / count);
        Vector2 w = MiscUtils.GetDirectionVector(phase);
        List<Projectile> projectiles = new List<Projectile>();
        for (int i = 0; i < count; i++)
        {
            projectiles.Add(EmitPartial(owner, w));
            w = MiscUtils.ComplexMultiply(w, w1);
        }
        return projectiles.ToArray();
    }

    private Projectile EmitPartial(Character owner, Vector2 direction)
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
        return p;
    }
}