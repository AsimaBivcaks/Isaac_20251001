using Godot;
using System;

public partial class Nerveending : Character
{
    [Export] public int MaxHP = 40;
    [Export] public NodePath meleeAreaPath;
    Area2D meleeArea;

    public override void _Ready()
    {
        base._Ready();

        meleeArea = GetNode<Area2D>(meleeAreaPath);

        AddBehavior(new CommonEnemyHPBehavior(this, MaxHP, Callable.From(() => {
            QueueFree();
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, meleeArea), BehaviorType.Melee);

        EndReady();
    }

}
