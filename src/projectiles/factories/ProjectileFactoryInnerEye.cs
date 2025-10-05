using Godot;
using System;

//direction.X: number of projectiles
//direction.Y: phase offset
public class ProjectileFactoryInnerEye : ProjectileFactoryModifier
{
    public int EmitCountPerSide = 0;
    public float EmitDisperse = MathF.PI/14;

    public ProjectileFactoryInnerEye(ProjectileFactory factory) : base(factory)
    {
    }
    
    public override void Emit(Character owner, Vector2 direction)
    {
        originalFactory.Emit(owner, direction);
        Vector2 w1 = MiscUtils.GetDirectionVector(EmitDisperse);
        Vector2 w = w1;
        for(int i = 1; i <= EmitCountPerSide; i++)
        {
            originalFactory.Emit(owner, MiscUtils.ComplexMultiply(direction, w1));
            originalFactory.Emit(owner, MiscUtils.ComplexMultiply(direction, MiscUtils.Conjugate(w1)));
            w1 = MiscUtils.ComplexMultiply(w1, w);
        }
    }
}