using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : Character
{
    [Export] public float velocityRefValue;
    [Export] public float emitCDRefValue;
    [Export] public float moveAcceleration;
    [Export] public NodePath animationController;

    public PlayerAnimationController anim;

    //private void GetItem(Item item)

    public override void _Ready()
    {
        base._Ready();

        anim = GetNode<PlayerAnimationController>(animationController);
        
        statusF["move_speed"] = 1.0f;
        statusF["fire_rate"] = 1.0f;
        statusF["damage"] = 1.0f;
        statusF["range"] = 1.0f;
        statusF["luck"] = 1.0f;

        //TEMP
        statusF["bombs"] = 99;

        AddBehavior(new PlayerMoveBehavior(this, moveAcceleration, velocityRefValue), BehaviorType.Move);
        AddBehavior(new PlayerEmitBehavior(this, emitCDRefValue), BehaviorType.Emit);
        AddBehavior(new PlayerHPBehavior(
            this,
            5,
            Callable.From(() => {
                GD.Print("Player Died");
            })
        ), BehaviorType.HP);
        AddBehavior(new PlayerBombBehavior(this), BehaviorType.PlayerBomb);
        
        projectileFactory = new ProjectileFactoryNormal(GD.Load<PackedScene>("res://scenes/projectiles/Tear.tscn"));

        EndReady();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        foreach (CharacterBehavior behavior in behaviors)
            behavior._Process(delta);
    }
}
