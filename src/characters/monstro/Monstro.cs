using Godot;
using System;

public partial class Monstro : BossCharacter, IShadow
{
    [Export] public float velocityRefValue;
    [Export] public NodePath animationController;
    [Export] public NodePath meleeArea;
    [Export] public float ZPosition { get; set; }

    public override void _Ready()
    {
        base._Ready();

        AddBehavior(new CommonEnemyHPBehavior(this, 500, Callable.From(() => {
            QueueFree();
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, GetNode<Area2D>(meleeArea)), BehaviorType.Melee);
        AddBehavior(new MonstroAIBehavior(this, animationController), BehaviorType.EnemyAI);

        EndReady();
    }
}