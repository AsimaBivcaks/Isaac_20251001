using Godot;
using System;
using System.Collections.Generic;

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
            WorldUtilsBlackboard.Get<List<Character>>("current_bosses").Remove(this);
            QueueFree();
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, GetNode<Area2D>(meleeArea)), BehaviorType.Melee);
        AddBehavior(new MonstroAIBehavior(this, animationController), BehaviorType.EnemyAI);

        EndReady();
    }
}