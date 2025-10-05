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
            uniqueFactory = new ProjectileFactoryInnerEye(player.projectileFactory);
            uniqueFactory.EmitCountPerSide = 0;
            uniqueFactory.EmitDisperse = EmitDisperse;
        }
        if (effectManager.GetEffectCount(this) == 0) //first one
        {
            uniqueFactory.originalFactory = player.projectileFactory;
            player.projectileFactory = uniqueFactory;
            player.statusF["fire_rate"] *= 0.7f;
        }
        uniqueFactory.EmitCountPerSide += EmitCountPerSide;
    }

    public override void OnRemove(Player player)
    {
        if (effectManager.GetEffectCount(this) == 1) //last one
        {
            var factory = player.projectileFactory;
            ProjectileFactoryModifier lastModifier = null;
            while (factory is ProjectileFactoryModifier modifier)
            {
                if (modifier == uniqueFactory)
                {
                    if (lastModifier != null)
                    {
                        lastModifier.originalFactory = modifier.originalFactory;
                    }
                    else
                    {
                        player.projectileFactory = modifier.originalFactory;
                    }
                    break;
                }
                lastModifier = modifier;
                factory = modifier.originalFactory;
            }
            player.statusF["fire_rate"] /= 0.7f;
        }
        else
        {
            uniqueFactory.EmitCountPerSide -= EmitCountPerSide;
        }
    }
}