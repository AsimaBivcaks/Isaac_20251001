using Godot;
using System;

public abstract class ProjectileFactoryModifier : ProjectileFactory
{
    public ProjectileFactory originalFactory;

    public ProjectileFactoryModifier(ProjectileFactory factory)
    {
        originalFactory = factory;
    }
}