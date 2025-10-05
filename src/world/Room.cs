using Godot;
using System;
using System.Collections.Generic;

public partial class Room : Node2D
{
    [Export] public NodePath[] SpawnGroups;

    public RoomSpace RoomSpace { get; private set; }
    private Player player = null;

    protected bool firstTimeEntered = true;
    
    public List<Character> trackedCharacters = new List<Character>();
    public List<TriggerObj> trackedItems = new List<TriggerObj>();

    public void AddCharacter(Character character)
    {
        trackedCharacters.Add(character);
        AddChild(character);
    }

    public void AddItem(Item item, Vector2 globalPosition)
    {
        TriggerObj obj = WorldUtilsSpawn.SpawnItem(this, globalPosition, item);
        if (obj != null)
        {
            trackedItems.Add(obj);
            AddChild(obj);
        }
    }

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

    public virtual void OnEnterRoom()
    {
        if (firstTimeEntered)
        {
            firstTimeEntered = false;

            foreach (NodePath path in SpawnGroups)
            {
                Node group = GetNode(path);
                foreach (Node child in group.GetChildren())
                    if (child is Spawn spawn)
                        spawn.Roll(this);
            }
        }
    }

    public virtual void OnExitRoom()
    {
    }
}