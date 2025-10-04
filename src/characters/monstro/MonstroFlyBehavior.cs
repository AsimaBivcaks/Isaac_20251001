using Godot;
using System;

public class MonstroFlyBehavior : CharacterBehavior
{
    private const float TOTAL_TIME = 1.0f;
    private const float END_ANIMATION_TIME = .3f;

    private MonstroAIBehavior stateMachine;
    private ProjectileFactory pfFountain;

    private float timer = 0f;
    private Vector2 Velocity;

    public MonstroFlyBehavior(Character _self, MonstroAIBehavior _stateMachine) : base(_self)
    {
        stateMachine = _stateMachine;
        pfFountain = new ProjectileFactoryFountain(GD.Load<PackedScene>(WorldUtilsPools.resourcePaths["proj_e_bloodtear"]));
    }

    private uint collisionLayersBackup;
    public override void PlugIn()
    {
        base.PlugIn();
        //GD.Print("MonstroFlyBehavior.PlugIn");
        stateMachine.anim.Play("fly1");
        stateMachine.anim.Squeeze(0.2f);
        timer = 0f;
        statusF["no_melee"] = 1;
        collisionLayersBackup = self.CollisionLayer;
        self.CollisionLayer = 0;
        Velocity = (WorldUtilsBlackboard.Get<Player>("player_instance").GlobalPosition - self.GlobalPosition) / TOTAL_TIME;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        timer += (float)delta;

        self.Position += Velocity * (float)delta;

        if(timer >= TOTAL_TIME)
        {
            statusF["no_melee"] = 0;
            pfFountain.Emit(self, new Vector2(13, WorldUtilsRandom.RandomRange(0, MathF.Tau)));
            stateMachine.SwitchState(MonstroAIBehavior.MAIBState.Idle);
            return;
        }

        if(timer >= TOTAL_TIME - END_ANIMATION_TIME && timer <= TOTAL_TIME - END_ANIMATION_TIME/2)
        {
            stateMachine.anim.Play("fly2");
        }

        if(timer >= TOTAL_TIME - END_ANIMATION_TIME/4)
        {
            statusF["no_melee"] = 0;
        }
    }

    public override void UnPlug()
    {
        base.UnPlug();

        self.CollisionLayer = collisionLayersBackup;
    }
}