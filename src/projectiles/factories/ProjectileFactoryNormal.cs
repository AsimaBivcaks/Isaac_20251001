using Godot;
using System;

public class ProjectileFactoryNormal : ProjectileFactory
{
    public PackedScene projectileScene;

    public ProjectileFactoryNormal(PackedScene scene)
    {
        projectileScene = scene;
    }
    
    public override void Emit(Player owner, Vector2 direction)
    {
        Projectile p = EmitInternal(owner,
            direction + SolveInertia(owner.Velocity.Normalized(), direction, .3f, .7f),
            projectileScene);
        p.statusF["damage"] = owner.statusF["damage"];
        p.statusF["range"] = owner.statusF["range"];
        p.statusF["speed"] = owner.statusF["move_speed"];
        //damn magic nums
        //idk how 2 export them elegantly
    }
}