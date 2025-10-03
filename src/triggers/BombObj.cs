using Godot;
using System;

public partial class BombObj : TriggerObj
{
    [Export] public float timerMax = 1.5f;
    [Export] public NodePath animationController;
    private float timer = 0.0f;
    //private bool active = false;

    private AnimationPlayer anim;

    public override void _Ready()
    {
        base._Ready();
        anim = GetNode<AnimationPlayer>(animationController);
        timer = timerMax;
        //active = true;
        anim.Play("default");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        //if(!active) return;

        timer -= (float)delta;
        if(timer <= 0.0f){
            //explode
            ProjectileFactoryBomb.Instance.BeforeEmit(mount);
            ProjectileFactoryBomb.Instance.Emit(null, Position);
            QueueFree();
        }
    }
}