using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class InnerEyeEffect : EffectItem
{
    [Export] public int EmitCountPerSide = 1;
    [Export] public float EmitDisperse = MathF.PI / 14;

    private static ProjectileFactoryInnerEye uniqueFactory = null;

    public override void OnActive(Player player)
    {
        if (uniqueFactory == null)
        {
            uniqueFactory = new ProjectileFactoryInnerEye();
            uniqueFactory.EmitCountPerSide = 0;
            uniqueFactory.EmitDisperse = EmitDisperse;
        }
        if (effectManager.GetEffectCount(this) == 0) //first one
        {
            player.statusF["fire_rate"] *= 0.7f;
        }
        uniqueFactory.EmitCountPerSide += EmitCountPerSide;
        if ((player.projectileFactory as ProjectileFactoryComposite).HasModifier(uniqueFactory))
            (player.projectileFactory as ProjectileFactoryComposite).AddModifier(uniqueFactory);
    }

    public override void OnRemove(Player player)
    {
        if (effectManager.GetEffectCount(this) == 1) //last one
        {
            uniqueFactory.EmitCountPerSide = 0;
            var factory = player.projectileFactory as ProjectileFactoryComposite;
            factory?.RemoveModifier(uniqueFactory);
            player.statusF["fire_rate"] /= 0.7f;
        }
        else
        {
            uniqueFactory.EmitCountPerSide -= EmitCountPerSide;
        }
    }
}