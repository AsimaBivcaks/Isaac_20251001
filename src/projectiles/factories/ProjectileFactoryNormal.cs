using Godot;
using System;

public class ProjectileFactoryNormal : ProjectileFactoryConcrete
{

    public ProjectileFactoryNormal(PackedScene scene) : base(scene)
    {
    }
    
    public override Projectile[] Emit(Character owner, Vector2 direction)
    {
        Projectile p = CreateProjectile(owner,
            direction + SolveInertia(owner.Velocity.Normalized(), direction, .3f, .7f),
            projectileScene);
        p.statusF["damage"] = owner.statusF["damage"];
        p.statusF["range"] = owner.statusF["range"];
        p.statusF["speed"] = owner.statusF["move_speed"];
        owner.Mount.AddChild(p);
        //damn magic nums
        //idk how 2 export them elegantly
        return new Projectile[] { p };
    }
}