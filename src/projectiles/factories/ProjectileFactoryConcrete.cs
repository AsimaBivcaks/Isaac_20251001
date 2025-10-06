using Godot;
using System;

public abstract class ProjectileFactoryConcrete : ProjectileFactory
{
    public PackedScene projectileScene;

    public ProjectileFactoryConcrete(PackedScene scene) : base()
    {
        projectileScene = scene;
    }
}