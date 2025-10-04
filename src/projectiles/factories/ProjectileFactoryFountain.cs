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
        Vector2 w1 = new Vector2(MathF.Cos(MathF.Tau/count), MathF.Sin(MathF.Tau/count));
        Vector2 w = new Vector2(MathF.Cos(phase), MathF.Sin(phase));
        for (int i = 0; i < count; i++)
        {
            EmitPartial(owner, w);
            w = new Vector2(
                w.X * w1.X - w.Y * w1.Y,
                w.X * w1.Y + w.Y * w1.X
            );
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