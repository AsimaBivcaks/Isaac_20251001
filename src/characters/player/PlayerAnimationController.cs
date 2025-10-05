using Godot;
using System;

public partial class PlayerAnimationController : Node2D
{
    [Export] private NodePath bodySprite;
    [Export] private NodePath headSprite;
    [Export] private NodePath specialSprite;
    [Export] private NodePath bodyAnimationPlayer;
    [Export] private NodePath headAnimationPlayer;
    [Export] private NodePath hitAnimationPlayer;

    private Sprite2D body;
    private Sprite2D head;
    private Sprite2D special;
    private AnimationPlayer bodyAnim;
    private AnimationTree headAnim;
    public AnimationPlayer HitAnim { get; private set; }
    private AnimationNodeStateMachinePlayback headState;

    public Player player { private get; set; }


    [Export] private NodePath statueSprite;
    private Sprite2D statue;


    public override void _Ready()
    {
        base._Ready();

        body = GetNode<Sprite2D>(bodySprite);
        head = GetNode<Sprite2D>(headSprite);
        special = GetNode<Sprite2D>(specialSprite);
        special.Visible = false;

        bodyAnim = GetNode<AnimationPlayer>(bodyAnimationPlayer);
        headAnim = GetNode<AnimationTree>(headAnimationPlayer);
        HitAnim = GetNode<AnimationPlayer>(hitAnimationPlayer);
        headState = (AnimationNodeStateMachinePlayback)headAnim.Get("parameters/playback");


        statue = GetNode<Sprite2D>(statueSprite);
        statue.Visible = false;
    }

    public void SetBody(Vector2 moveDir)
    {
        if (moveDir.LengthSquared() < .1f)
        {
            bodyAnim.Play("idle");
            return;
        }

        if (Math.Abs(moveDir.Y) > .2f)
        {
            bodyAnim.Play("walk_v");
        } else {
            body.FlipH = moveDir.X < 0;
            bodyAnim.Play("walk_h");
        }
    }

    public void SetHead(Vector2 facing,float velocity , bool emitted=false)
    {
        if (velocity < 15f){
            headState.Travel("d");
            return;
        }

        String animname = "s";
        if (Math.Abs(facing.Y) > .1f){
            if (facing.Y < -.1f)
                animname = "u";
            else
                animname = "d";
        } else {
            head.FlipH = facing.X < 0;
        }

        if (emitted) //blink when emitting
            headState.Travel(animname + "e");
        else
            headState.Travel(animname);
    }

    public void ShowSpecial(int frame, float duration, Callable? callback=null)
    {
        special.Frame = frame;
        special.Visible = true;
        body.Visible = false;
        head.Visible = false;
        player.statusF["pause"] = 1;
        GetTree().CreateTimer(duration).Timeout += () => {
            player.statusF["pause"] = 0;
            callback?.Call();
            special.Visible = false;
            body.Visible = true;
            head.Visible = true;
        };
    }
    
    public void OnSwitchingRooms(float duration, Vector2 newPos)
    {
        body.Visible = false;
        head.Visible = false;
        special.Visible = false;
        player.statusF["pause"] = 1;
        GetTree().CreateTimer(duration).Timeout += () => {
            player.GlobalPosition = newPos;
            player.statusF["pause"] = 0;
            body.Visible = true;
            head.Visible = true;
        };
    }
    
    public void ShowDeath(int frame, float duration, Callable? callback=null)
    {
        special.Frame = frame;
        special.Visible = true;
        body.Visible = false;
        head.Visible = false;
        player.statusF["pause"] = 1;
        GetTree().CreateTimer(duration).Timeout += () => {
            //player.statusF["pause"] = 0;
            callback?.Call();
        };
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (player != null && player.statusF.ContainsKey("GLB_invincible") && player.statusF["GLB_invincible"] > .5f)
        {
            Visible = false;
            statue.Visible = true;
        }
        else
        {
            Visible = true;
            statue.Visible = false;
        }
    }
}
