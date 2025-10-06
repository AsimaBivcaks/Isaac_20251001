using Godot;
using System;

//When a BombObj reaches zero timer, it calls this factory to emit a bomb projectile,
// which is basically a explosion that deals damage in an area then disappears immediately.
//Emit: owner=null, direction=position
public class ProjectileFactoryBomb : ProjectileFactoryConcrete
{
    public static ProjectileFactoryBomb Instance = new ProjectileFactoryBomb(
        WorldUtilsPools.GetResource<PackedScene>("proj_explosion")
    );

    public ProjectileFactoryBomb(PackedScene scene) : base(scene)
    {
    }
    
    private Node TempMount;
    public void BeforeEmit(Node mount){
        TempMount = mount;
    }
    public override Projectile[] Emit(Character useless, Vector2 pos)
    {
        Projectile p = projectileScene.Instantiate<Projectile>();
        p.Position = pos;
        p.Velocity = new Vector2();
        TempMount.AddChild(p);
        return new Projectile[]{p};
    }
}