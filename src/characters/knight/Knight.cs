using Godot;
using System;

public partial class Knight : Character
{
    [Export] public float velocityRefValue1 = 100f;
    [Export] public float velocityRefValue2 = 350f;
    [Export] public float moveAcceleration = 1000f;

    [Export] public int MaxHP = 40;
    [Export] public NodePath MeleeAreaPath;

    [Export] public NodePath SprHeadPath;
    [Export] public NodePath SprBodyPath;

    [Export] public NodePath PlayerDetectorPath;

    private Area2D meleeArea;
    private Sprite2D sprHead;
    private AnimatedSprite2D sprBody;
    public RayCast2D playerDetector {get; private set;}

    private float detectRange;

    public override void _Ready()
    {
        base._Ready();

        meleeArea = GetNode<Area2D>(MeleeAreaPath);

        sprHead = GetNode<Sprite2D>(SprHeadPath);
        sprBody = GetNode<AnimatedSprite2D>(SprBodyPath);
        playerDetector = GetNode<RayCast2D>(PlayerDetectorPath);
        playerDetector.Enabled = true;
        detectRange = playerDetector.TargetPosition.Length();

        AddBehavior(new KnightHPBehavior(this, MaxHP, Callable.From(() => {
            QueueFree();
        })), BehaviorType.HP);
        AddBehavior(new CommonEnemyMeleeBehavior(this, meleeArea), BehaviorType.Melee);
        AddBehavior(
            new KnightMoveBehavior(this,
                velocityRefValue1,
                velocityRefValue2,
                moveAcceleration), BehaviorType.Move); //omg this is so ugly

        EndReady();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        UpdateAnimation();
    }

    public void UpdateAnimation()
    {
        Vector2 facing = statusV["facing"];
        String bodyanim = "";
        if( facing.Y > .3f )
        {
            bodyanim = "v";
            sprHead.Frame = 0;
            sprHead.FlipH = false;
            sprBody.FlipH = false;
            playerDetector.TargetPosition = new Vector2(0, detectRange);
        }
        else if( facing.Y < -.3f )
        {
            bodyanim = "v";
            sprHead.Frame = 2;
            sprHead.FlipH = false;
            sprBody.FlipH = false;
            playerDetector.TargetPosition = new Vector2(0, -detectRange);
        }
        else if( facing.X > .3f )
        {
            bodyanim = "h";
            sprHead.Frame = 1;
            sprHead.FlipH = false;
            sprBody.FlipH = false;
            playerDetector.TargetPosition = new Vector2(detectRange, 0);
        }
        else if( facing.X < -.3f )
        {
            bodyanim = "h";
            sprHead.Frame = 1;
            sprHead.FlipH = true;
            sprBody.FlipH = true;
            playerDetector.TargetPosition = new Vector2(-detectRange, 0);
        }
        if (statusF["dash"] > 0.5f)
            bodyanim += "fast";
        
        sprBody.Play(bodyanim);
        //crazy
    }
}
