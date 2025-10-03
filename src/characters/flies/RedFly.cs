using Godot;
using System;

public partial class RedFly : Character
{
    [Export] public int MaxHP = 10;
    [Export] public NodePath meleeAreaPath;
    [Export] public float velocityRefValue = 20f;
    private Area2D meleeArea;

    public override void _Ready()
    {
        base._Ready();

        meleeArea = GetNode<Area2D>(meleeAreaPath);

        AddBehavior(new CommonEnemyHPBehavior(this, MaxHP, Callable.From(() => {
            QueueFree();
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, meleeArea), BehaviorType.Melee);
        AddBehavior(new RedFlyMoveBehavior(this, velocityRefValue), BehaviorType.Move);

        EndReady();
    }
}
