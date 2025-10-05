using Godot;
using System;
using System.Collections.Generic;

public partial class Room : Node2D
{
    public RoomSpace RoomSpace { get; private set; }
    private Player player = null;
    
    public List<Character> trackedCharacters = new List<Character>();
    public List<TriggerObj> trackedItems = new List<TriggerObj>();

    public override void _Ready()
    {
        base._Ready();
        RoomSpace = GetNode<RoomSpace>("RoomSpace");
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (player == null)
        {
            player = WorldUtilsBlackboard.Get<Player>("player_instance");
            if (player == null)
                return;
        }
    }
}