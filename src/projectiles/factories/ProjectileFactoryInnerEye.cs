using Godot;
using System;
using System.Collections.Generic;

//direction.X: number of projectiles
//direction.Y: phase offset
public class ProjectileFactoryInnerEye : ProjectileFactoryModifier
{
    public int EmitCountPerSide = 0;
    public float EmitDisperse = MathF.PI/14;

    public override Projectile[] Emit(Character owner, Vector2 direction)
    {
        List<Projectile> projectiles = new List<Projectile>( originalFactory.Emit(owner, direction) );
        Vector2 w1 = MiscUtils.GetDirectionVector(EmitDisperse);
        Vector2 w = w1;
        for(int i = 1; i <= EmitCountPerSide; i++)
        {
            projectiles.AddRange(originalFactory.Emit(owner, MiscUtils.ComplexMultiply(direction, w1)));
            projectiles.AddRange(originalFactory.Emit(owner, MiscUtils.ComplexMultiply(direction, MiscUtils.Conjugate(w1))));
            w1 = MiscUtils.ComplexMultiply(w1, w);
        }
        return projectiles.ToArray();
    }
}