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
    [Export] public float VelocityRefValue;
    [Export] public float EmitCDRefValue;
    [Export] public float MoveAcceleration;
    [Export] public NodePath AnimationController;
    [Export] public NodePath InteractAreaPath;

    public PlayerAnimationController anim;
    public Area2D interactArea;

    //private void GetItem(Item item)

    public override void _Ready()
    {
        base._Ready();

        anim = GetNode<PlayerAnimationController>(AnimationController);
        anim.player = this;

        interactArea = GetNode<Area2D>(InteractAreaPath);
        
        statusF["move_speed"] = 1.0f;
        statusF["fire_rate"] = 1.0f;
        statusF["damage"] = 0.0f;
        statusF["range"] = 1.0f;
        statusF["luck"] = 1.0f;

        WorldUtilsBlackboard.Set("player_instance", this);

        AddBehavior(new PlayerMoveBehavior(this, MoveAcceleration, VelocityRefValue), BehaviorType.Move);
        AddBehavior(new PlayerEmitBehavior(this, EmitCDRefValue), BehaviorType.Attack);
        AddBehavior(new PlayerHPBehavior(
            this,
            6,
            Callable.From(() => {
                anim.ShowDeath(3, 3f, Callable.From(() => {
                    //GetTree().ChangeSceneToFile("res://scenes/menus/GameOver.tscn");
                    GetTree().Quit();
                }));
            })
        ), BehaviorType.HP);
        AddBehavior(new PlayerBombBehavior(this), BehaviorType.PlayerBomb);
        AddBehavior(new PlayerInteractBehavior(this, interactArea), BehaviorType.PlayerInteract);
        AddBehavior(new PlayerKeyManagementBehavior(this), BehaviorType.PlayerKeyManagement);
        AddBehavior(new PlayerMoneyManagementBehavior(this), BehaviorType.PlayerMoneyManagement);
        AddBehavior(new PlayerUsableManagementBehavior(this), BehaviorType.PlayerUsableManagement);
        AddBehavior(new PlayerEffectManagementBehavior(this), BehaviorType.PlayerEffectManagement);
        
        var baseFactory = new ProjectileFactoryNormal(WorldUtilsPools.GetResource<PackedScene>("proj_tear"));
        projectileFactory = new ProjectileFactoryComposite(baseFactory);

        EndReady();
    }
}
