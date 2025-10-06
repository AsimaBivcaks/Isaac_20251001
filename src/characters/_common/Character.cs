//Status for all Character:
//  pause: float, 1 for paused, 0 for not paused
//  inertia: Vector2, external forces applied to the character, decays over time

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public abstract partial class Character : CharacterBody2D
{
    public Dictionary<string, float> statusF = new Dictionary<string, float>();
    public Dictionary<string, Vector2> statusV = new Dictionary<string, Vector2>();

    //personally i would like to directly implement most of the basic processes in some methods in this class
    //but honestly the task of implementing item "Chocolate Milk" really scared the shxt out of me
    readonly protected List<CharacterBehavior> behaviors = new List<CharacterBehavior>();
    readonly protected Dictionary<BehaviorType, CharacterBehavior> behaviorMap = new Dictionary<BehaviorType, CharacterBehavior>();
    private List<CharacterBehavior> aboutToRemove = new List<CharacterBehavior>();
    private List<CharacterBehavior> aboutToAdd = new List<CharacterBehavior>();

    //ALWAYS CALL PlugIn & UnPlug WHEN CHANGING THESE MANUALLY
    
    //emitBehavior's about when to emit. "how to emit" is mostly in ProjectileFactory

    public ProjectileFactory projectileFactory;
    public Node Mount; //the node for summon to attach to

    public void AddBehavior(CharacterBehavior behavior, BehaviorType type, bool immediate=true)
    {
        behavior.type = type;
        if(immediate)
        {
            behaviors.Add(behavior);
            behaviorMap[behavior.type] = behavior;
            behavior.PlugIn();
        }
        else
        {
            aboutToAdd.Add(behavior);
        }
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
        aboutToRemove.Add(behavior);
    }

    public void RemoveBehavior(BehaviorType behaviorType)
    {
        if(behaviorMap.ContainsKey(behaviorType)){
            var behavior = behaviorMap[behaviorType];
            aboutToRemove.Add(behavior);
        }
    }

    public override void _Ready()
    {
        base._Ready();

        //Maybe TEMP
        Mount = GetParent<Node>();

        statusV["inertia"] = new Vector2(0, 0);
        statusF["pause"] = 0; //1 for paused, 0 for not paused
        
    }
    
    protected void EndReady(){
        foreach (CharacterBehavior behavior in behaviors)
            behavior._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        statusV["inertia"] = statusV["inertia"].MoveToward(new Vector2(0, 0), 80 * (float)delta);

        foreach (CharacterBehavior behavior in behaviors)   
            behavior._Process(delta);
        
        foreach (CharacterBehavior behavior in aboutToAdd)
        {
            behaviors.Add(behavior);
            behaviorMap[behavior.type] = behavior;
            behavior.PlugIn();
        }

        foreach (CharacterBehavior behavior in aboutToRemove)
        {
            behaviors.Remove(behavior);
            behaviorMap.Remove(behavior.type);
            behavior.UnPlug();
        }

        aboutToAdd.Clear();
        aboutToRemove.Clear();
    }

    public void Move(double delta, bool considerinertia=true){
        Vector2 v2 = Velocity;
        if(considerinertia)
            Velocity += statusV["inertia"];
        MoveAndSlide();
        Velocity = v2;
    }
}
