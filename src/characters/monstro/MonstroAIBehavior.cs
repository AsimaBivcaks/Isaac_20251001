using Godot;
using System;
using System.Collections.Generic;
using System.Data;

public class MonstroAIBehavior : CharacterBehavior
{
    public enum MAIBState
    {
        Empty,
        Idle,
        RandomAttack,
        Jump,
        Fly,
        Fire,
    }

    private MAIBState currentState = MAIBState.Empty;
    private Dictionary<MAIBState, CharacterBehavior> stateBehaviors = new();
    
    public MonstroAnimationController anim{ get; private set; }
    //private AnimationNodeStateMachinePlayback anim;


    public MonstroAIBehavior(Character _self, NodePath animationController) : base(_self)
    {
        anim = self.GetNode<MonstroAnimationController>(animationController);
    }

    public override void _Ready()
    {
        base._Ready();

        //anim = (AnimationNodeStateMachinePlayback)animCtrl.Get("parameters/playback");

        stateBehaviors.Add(MAIBState.Idle, new MonstroWaitBehavior(self, this));
        stateBehaviors.Add(MAIBState.Fire, new MonstroFireBehavior(self, this));
        stateBehaviors.Add(MAIBState.Fly, new MonstroFlyBehavior(self, this));
        stateBehaviors.Add(MAIBState.Jump, new MonstroJumpBehavior(self, this));

        SwitchState(MAIBState.Idle);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(stateBehaviors.ContainsKey(currentState))
            stateBehaviors[currentState]._Process(delta);
    }

    public void SwitchState(MAIBState state)
    {
        if(currentState == state) return;
        if(state == MAIBState.RandomAttack)
        {
            int val = WorldUtilsRandom.RandomiRange(0, 3);
            switch(val)
            {
                case 0:
                    state = MAIBState.Jump;
                    break;
                case 1:
                    state = MAIBState.Fly;
                    break;
                case 2:
                    state = MAIBState.Fire;
                    break;
            }
        }
        if(stateBehaviors.ContainsKey(currentState))
        {
            stateBehaviors[currentState].UnPlug();
        }
        currentState = state;
        if(stateBehaviors.ContainsKey(currentState))
        {
            stateBehaviors[currentState].PlugIn();
        }
    }
}