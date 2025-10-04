using Godot;
using System;

public class MonstroWaitBehavior : CharacterBehavior
{
    private MonstroAIBehavior stateMachine;
    bool endflag = false;

    public MonstroWaitBehavior(Character _self, MonstroAIBehavior _stateMachine) : base(_self)
    {
        stateMachine = _stateMachine;
    }

    public override void PlugIn()
    {
        base.PlugIn();
        stateMachine.anim.Play("wait");
        stateMachine.anim.AnimationFinished += EndWait;
        stateMachine.anim.Squeeze(-0.1f);
        endflag = false;
    }

    public void EndWait(StringName animName)
    {
        if(endflag) return;
        endflag = true;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if(endflag && WorldUtilsRandom.Chance(.4f))
        {
            stateMachine.SwitchState(MonstroAIBehavior.MAIBState.RandomAttack);
        }
    }

    public override void UnPlug()
    {
        base.UnPlug();
        stateMachine.anim.AnimationFinished -= EndWait;
    }
}