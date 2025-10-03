using Godot;
using System;

//When a BombObj reaches zero timer, it calls this factory to emit a bomb projectile,
// which is basically a explosion that deals damage in an area then disappears immediately.
//Emit: owner=null, direction=position
public class ProjectileFactoryBomb : ProjectileFactory
{
    public static ProjectileFactoryBomb Instance = new ProjectileFactoryBomb(
        GD.Load<PackedScene>("res://scenes/projectiles/explosion.tscn")
    );
    public PackedScene projectileScene;

    public ProjectileFactoryBomb(PackedScene scene)
    {
        projectileScene = scene;
    }
    
    private Node TempMount;
    public void BeforeEmit(Node mount){
        TempMount = mount;
    }
    public override void Emit(Character useless, Vector2 pos)
    {
        Projectile p = projectileScene.Instantiate<Projectile>();
        p.Position = pos;
        p.Velocity = new Vector2();
        TempMount.AddChild(p);
    }
}