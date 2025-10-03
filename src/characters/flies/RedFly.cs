using Godot;
using System;

public partial class RedFly : Character
{
    [Export] public int MaxHP = 10;
    [Export] public NodePath meleeAreaPath;
    [Export] public NodePath sprPath;
    [Export] public float velocityRefValue = 20f;
    private Area2D meleeArea;
    private AnimatedSprite2D spr;

    public override void _Ready()
    {
        base._Ready();

        meleeArea = GetNode<Area2D>(meleeAreaPath);
        spr = GetNode<AnimatedSprite2D>(sprPath);

        AddBehavior(new CommonEnemyHPBehavior(this, MaxHP, Callable.From(() => {
            spr.Play("death");
            statusF["pause"] = 1f;
            spr.AnimationFinished += QueueFree;
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, meleeArea), BehaviorType.Melee);
        AddBehavior(new RedFlyMoveBehavior(this, velocityRefValue), BehaviorType.Move);

        EndReady();
    }
}
