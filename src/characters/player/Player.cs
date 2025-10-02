using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public enum BehaviorType{
    Move,
    Emit,
    HP,
}

public partial class Player : CharacterBody2D
{
    public Dictionary<string, float> statusF = new Dictionary<string, float>();
    public Dictionary<string, Vector2> statusV = new Dictionary<string, Vector2>();

    //personally i would like to directly implement most of the basic processes in some methods in Player
    //but honestly the task of implementing item "Chocolate Milk" really scared the shxt out of me
    readonly private List<CharacterBehavior> behaviors = new List<CharacterBehavior>();
    readonly private Dictionary<BehaviorType, CharacterBehavior> behaviorMap = new Dictionary<BehaviorType, CharacterBehavior>();

    //ALWAYS CALL PlugIn & UnPlug WHEN CHANGING THESE MANUALLY
    
    //emitBehavior's about when to emit. "how to emit" is mostly in ProjectileFactory

    public ProjectileFactory projectileFactory;

    [Export] public float velocityRefValue;
    [Export] public float emitCDRefValue;
    [Export] public float moveAcceleration;
    [Export] public NodePath animationController;

    public PlayerAnimationController anim;


    //private void GetItem(Item item)

    public void AddBehavior(CharacterBehavior behavior, BehaviorType type)
    {
        behaviors.Add(behavior);
        behaviorMap[type] = behavior;
        behavior.PlugIn();
    }

    public CharacterBehavior GetBehavior(BehaviorType behaviorType)
    {
        if(behaviorMap.ContainsKey(behaviorType))
            return behaviorMap[behaviorType];
        return null;
    }

    public T GetBehavior<T>(BehaviorType behaviorType) where T : CharacterBehavior
    {
        if(behaviorMap.ContainsKey(behaviorType))
            return (T)behaviorMap[behaviorType];
        return null;
    }

    public void RemoveBehavior(CharacterBehavior behavior)
    {
        behaviors.Remove(behavior);
        behaviorMap.Remove(behaviorMap.First(kv => kv.Value == behavior).Key);
        behavior.UnPlug();
    }

    public void RemoveBehavior(BehaviorType behaviorType)
    {
        if(behaviorMap.ContainsKey(behaviorType)){
            var behavior = behaviorMap[behaviorType];
            behaviors.Remove(behavior);
            behaviorMap.Remove(behaviorType);
            behavior.UnPlug();
        }
    }

    public override void _Ready()
    {
        base._Ready();

        anim = GetNode<PlayerAnimationController>(animationController);
        
        statusF["move_speed"] = 1.0f;
        statusF["fire_rate"] = 1.0f;
        statusF["damage"] = 1.0f;
        statusF["range"] = 1.0f;
        statusF["luck"] = 1.0f;

        AddBehavior(new PlayerMoveBehavior(this, moveAcceleration, velocityRefValue), BehaviorType.Move);
        AddBehavior(new PlayerEmitBehavior(this, emitCDRefValue), BehaviorType.Emit);
        AddBehavior(new PlayerHPBehavior(
            this,
            5,
            Callable.From(() => {
                GD.Print("Player Died");
            })
        ), BehaviorType.HP);
        
        projectileFactory = new ProjectileFactoryNormal(GD.Load<PackedScene>("res://scenes/projectiles/Tear.tscn"));

        foreach (CharacterBehavior behavior in behaviors)
            behavior._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        foreach (CharacterBehavior behavior in behaviors)
            behavior._Process(delta);
    }
}
