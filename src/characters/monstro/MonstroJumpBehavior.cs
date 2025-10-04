using Godot;
using System;

public class MonstroJumpBehavior : CharacterBehavior
{
    private const float TOTAL_TIME = 1f;

    private MonstroAIBehavior stateMachine;
    
    private Vector2 Velocity;

    public MonstroJumpBehavior(Character _self, MonstroAIBehavior _stateMachine) : base(_self)
    {
        stateMachine = _stateMachine;
    }

    private uint collisionLayersBackup;
    public override void PlugIn()
    {
        base.PlugIn();
        stateMachine.anim.Play("jump");
        stateMachine.anim.Squeeze(0.1f);
        stateMachine.anim.AnimationFinished += EndJump;    

        Velocity = (WorldUtilsBlackboard.Get<Player>("player_instance").GlobalPosition - self.GlobalPosition) / TOTAL_TIME;
        if( Velocity.Length() > ((Monstro)self).velocityRefValue )
        {
            Velocity = Velocity.Normalized() * ((Monstro)self).velocityRefValue;
        }
        statusF["no_melee"] = 1;
        collisionLayersBackup = self.CollisionLayer;
        self.CollisionLayer = 0;
    }

    private void EndJump(StringName animName)
    {
        self.CollisionLayer = collisionLayersBackup;
        statusF["no_melee"] = 0;
        stateMachine.SwitchState(MonstroAIBehavior.MAIBState.Idle);
    }

    public override void _Process(double delta)
    {
        self.Velocity = Velocity;
        self.MoveAndSlide();
        base._Process(delta);
    }

    public override void UnPlug()
    {
        base.UnPlug();
        self.Velocity = Vector2.Zero;
        stateMachine.anim.AnimationFinished -= EndJump;
    }
}