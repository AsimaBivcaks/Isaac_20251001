//Exclusive status for Player:
//    move_speed: float, multiplier for movement speed
//    fire_rate: float, multiplier for firing rate
//    damage: float, additional damage added to each projectile
//    range: float, multiplier for projectile range
//    luck: float, multiplier for luck-based effects
//    bombs: float, number of bombs the player has
//    emitted_this_frame: float, 1 if the player has emitted a projectile this frame, otherwise 0

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Player : Character
{
    [Export] public float velocityRefValue;
    [Export] public float emitCDRefValue;
    [Export] public float moveAcceleration;
    [Export] public NodePath animationController;

    public PlayerAnimationController anim;

    //private void GetItem(Item item)

    public override void _Ready()
    {
        base._Ready();

        anim = GetNode<PlayerAnimationController>(animationController);
        anim.player = this;
        
        statusF["move_speed"] = 1.0f;
        statusF["fire_rate"] = 1.0f;
        statusF["damage"] = 0.0f;
        statusF["range"] = 1.0f;
        statusF["luck"] = 1.0f;

        //TEMP
        statusF["bombs"] = 99;

        //TEMP
        WorldUtilsRandom.Init(12345);
        WorldUtilsRng.Init(12345 * 31);

        //maybe TEMP
        WorldUtilsBlackboard.Set("player_instance", this);
        WorldUtilsBlackboard.Set("decision_frequency", .2f);

        AddBehavior(new PlayerMoveBehavior(this, moveAcceleration, velocityRefValue), BehaviorType.Move);
        AddBehavior(new PlayerEmitBehavior(this, emitCDRefValue), BehaviorType.Attack);
        AddBehavior(new PlayerHPBehavior(
            this,
            6,
            Callable.From(() => {
                anim.ShowSpecial(3, 3f, Callable.From(() => {
                    //GetTree().ChangeSceneToFile("res://scenes/menus/GameOver.tscn");
                    GetTree().Quit();
                }));
            })
        ), BehaviorType.HP);
        AddBehavior(new PlayerBombBehavior(this), BehaviorType.PlayerBomb);
        
        projectileFactory = new ProjectileFactoryNormal(GD.Load<PackedScene>("res://scenes/projectiles/Tear.tscn"));

        EndReady();
    }
}
