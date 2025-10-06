using Godot;
using System;
using System.Collections.Generic;

public class ProjectileFactoryComposite : ProjectileFactory
{
    public ProjectileFactoryConcrete baseFactory;
    public List<ProjectileFactoryModifier> modifiers;

    public ProjectileFactoryComposite(ProjectileFactoryConcrete baseFactory, params ProjectileFactoryModifier[] modifiers)
    {
        this.baseFactory = baseFactory;
        this.modifiers = new List<ProjectileFactoryModifier>(modifiers);
    }

    public ProjectileFactoryComposite()
    {
        this.baseFactory = null;
        this.modifiers = new List<ProjectileFactoryModifier>();
    }

    public void AddModifier(ProjectileFactoryModifier modifier)
    {
        if (modifier != null)
        {
            modifiers.Add(modifier);
        }
    }

    public bool HasModifier(ProjectileFactoryModifier modifier)
    {
        if (modifier != null)
        {
            return modifiers.Contains(modifier);
        }
        return false;
    }

    public void RemoveModifier(ProjectileFactoryModifier modifier)
    {
        if (modifier != null)
        {
            modifiers.Remove(modifier);
        }
    }

    public override Projectile[] Emit(Character owner, Vector2 direction)
    {
        if (baseFactory == null) return new Projectile[0];

        ProjectileFactory last = baseFactory;
        foreach (var modifier in modifiers)
        {
            if (modifier == null) continue;
            modifier.originalFactory = last;
            last = modifier;
        }
        return last.Emit(owner, direction);
    }
}