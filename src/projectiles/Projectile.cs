using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Projectile : Node2D
{
    public Dictionary<string, float> statusF = new Dictionary<string, float>();
    //public Dictionary<string, Vector2> statusV = new Dictionary<string, Vector2>();
    
    [ExportGroup("Attributes")]
    [Export] public float SpeedRefValue = 250.0f;
    [Export] public float RangeRefValue = 100.0f;
    [Export] public int DamageRefValue = 1;
    [Export] public float KnockbackRefValue = 0.0f;
    [Export] public NodePath animationController;

    [ExportGroup("Collision")]
    [Export(PropertyHint.Layers2DPhysics)] public uint wallLayer = 1;
    [Export(PropertyHint.Layers2DPhysics)] public uint targetLayer = 8;
    [Export] public NodePath hitboxArea;
    [Export] public bool destroyWhenHit = true;

    protected AnimationPlayer anim;
    protected Area2D hitbox;
    public CharacterBody2D attacker;

    public Vector2 Velocity = Vector2.Zero;
    protected Vector2 xyPosition;

    protected abstract void HitTarget(Character target);
    protected abstract void Destroy();

    public override void _Ready()
    {
        base._Ready();
        anim = GetNode<AnimationPlayer>(animationController);

        hitbox = GetNode<Area2D>(hitboxArea);
        hitbox.CollisionLayer = wallLayer;
        hitbox.CollisionMask = wallLayer | targetLayer;
        hitbox.BodyEntered += (Node2D body) => {
            if(body is CharacterBody2D target){
                if((target.CollisionLayer & targetLayer) != 0){
                    HitTarget((Character)target);
                    if(destroyWhenHit)
                        Destroy();
                }
                else if ((target.CollisionLayer & wallLayer) != 0){
                    Destroy();
                }
            }
        };

        statusF["distance_traveled"] = 0.0f;
        statusF["disabled"] = 0;

        if(!statusF.ContainsKey("range"))
            statusF["range"] = 1.0f;
        statusF["range"] *= RangeRefValue;
        if(!statusF.ContainsKey("damage"))
            statusF["damage"] = .0f;
        statusF["damage"] =(int)( DamageRefValue * Mathf.Sqrt(1 + 1.2f * statusF["damage"]) );
        if(!statusF.ContainsKey("speed"))
            statusF["speed"] = 1.0f;
        statusF["speed"] *= SpeedRefValue;

        xyPosition = Position;
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        statusF["distance_traveled"] = 0.0f;
        statusF["disabled"] = 0;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if(statusF["disabled"] > 0.5f)
            return;
        
        Vector2 moveVec = Velocity * (float)delta;
        statusF["distance_traveled"] += moveVec.Length();
        xyPosition += moveVec;

        if(statusF["distance_traveled"] >= statusF["range"])
            Destroy();
    }

    public void Disable()
    {
        statusF["disabled"] = 1;
        Velocity = Vector2.Zero;
        hitbox.Monitoring = false;
        hitbox.Monitorable = false;
    }
}
