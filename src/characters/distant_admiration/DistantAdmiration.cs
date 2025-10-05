using Godot;
using System;

public partial class DistantAdmiration : Character
{
    [Export] public NodePath MeleeAreaPath;
    public Player player;

    public override void _Ready()
    {
        base._Ready();

        player = WorldUtilsBlackboard.Get<Player>("player_instance");

        AddBehavior(new DistantAdmirationMeleeBehavior(this, GetNode<Area2D>(MeleeAreaPath)), BehaviorType.Melee);
        AddBehavior(new DistantAdmirationMoveBehavior(this, player), BehaviorType.Move);

        EndReady();
    }
}