using Godot;
using System;

public partial class PlayerAnimationController : Node2D
{
    [Export] private NodePath bodySprite;
    [Export] private NodePath headSprite;
    [Export] private NodePath specialSprite;
    [Export] private NodePath bodyAnimationPlayer;
    [Export] private NodePath headAnimationPlayer;

    private Sprite2D body;
    private Sprite2D head;
    private Sprite2D special;
    private AnimationPlayer bodyAnim;
    private AnimationTree headAnim;
    private AnimationNodeStateMachinePlayback headState;
    public override void _Ready()
    {
        base._Ready();

        body = GetNode<Sprite2D>(bodySprite);
        head = GetNode<Sprite2D>(headSprite);
        special = GetNode<Sprite2D>(specialSprite);
        special.Visible = false;

        bodyAnim = GetNode<AnimationPlayer>(bodyAnimationPlayer);
        headAnim = GetNode<AnimationTree>(headAnimationPlayer);
        headState = (AnimationNodeStateMachinePlayback)headAnim.Get("parameters/playback");
    }

    public void SetBody(Vector2 moveDir)
    {
        if (moveDir.Length() < .2f)
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
}
