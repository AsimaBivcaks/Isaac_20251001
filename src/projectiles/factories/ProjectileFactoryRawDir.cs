using Godot;
using System;

public class ProjectileFactoryRawDir : ProjectileFactory
{
    public PackedScene projectileScene;

    public ProjectileFactoryRawDir(PackedScene scene)
    {
        projectileScene = scene;
    }
    
    public override void Emit(Character owner, Vector2 direction)
    {
        Projectile p = CreateProjectile(owner,
            direction,
            projectileScene);
        p.Velocity = direction;
        p.Position = owner.Position + new Vector2(0,-3) + WorldUtilsRandom.RandomInDisc(10f);
        owner.Mount.AddChild(p);
    }
}