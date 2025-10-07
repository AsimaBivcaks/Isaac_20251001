using Godot;
using System;

public class MonstroFireBehavior : CharacterBehavior
{
    private const float fireInterval = .05f;

    private float fireTimer = 0f;

    private MonstroAIBehavior stateMachine;
    private ProjectileFactory pfRawDir;

    public MonstroFireBehavior(Character _self, MonstroAIBehavior _stateMachine) : base(_self)
    {
        stateMachine = _stateMachine;
        pfRawDir = new ProjectileFactoryRawDir(
            WorldUtilsPools.GetResource<PackedScene>("proj_e_aired_bloodtear")
        );
    }

    public override void PlugIn()
    {
        base.PlugIn();

        stateMachine.anim.Play("fire");
        stateMachine.anim.AnimationFinished += EndFire;
    }

    public void EndFire(StringName animName)
    {
        stateMachine.SwitchState(MonstroAIBehavior.MAIBState.Idle);
    }

    public override void UnPlug()
    {
        base.UnPlug();
        stateMachine.anim.AnimationFinished -= EndFire;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(fireTimer > 0f)
        {
            fireTimer -= (float)delta;
            return;
        }
        fireTimer = fireInterval + WorldUtilsRandom.RandomRange(-.02f, .02f);
        pfRawDir.Emit(self,
            WorldUtilsBlackboard.Get<Character>("player_instance").GlobalPosition
            + WorldUtilsRandom.RandomInDisc(40f)
        );
    }
}