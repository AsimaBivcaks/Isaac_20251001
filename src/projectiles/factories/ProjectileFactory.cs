// Base class for projectile emission behaviors

using Godot;
using System;

public abstract class ProjectileFactory
{
    public abstract Projectile[] Emit(Character owner, Vector2 direction);

    protected Projectile CreateProjectile(Character owner, Vector2 psu_dir, PackedScene projectileScene)
    {
        Projectile p = projectileScene.Instantiate<Projectile>();
        p.Position = owner.Position + psu_dir.Normalized()*2 + new Vector2(0, -1);
        p.Velocity = psu_dir * p.SpeedRefValue;
        //owner.Mount.AddChild(p);
        return p;
    }

    
    // eV & eL should be normalized
    // lambda: 0 ~ 1, mu: 0 ~ 1
    public static Vector2 SolveInertia(Vector2 eV, Vector2 eL,float lambda, float mu){
        Vector2 eN = new Vector2(-eL.Y, eL.X);
        return eL * lambda * eL.Dot(eV) + eN * mu * eN.Dot(eV);
    }
}