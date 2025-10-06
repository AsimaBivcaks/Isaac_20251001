using Godot;
using System;
using System.Collections.Generic;

public partial class Room : Node2D
{
    [Export] public NodePath[] SpawnGroups;

    public Vector2I GridPosition; //the grid position of the room in the world

    [Export] public RoomSpace roomSpace;

    private Player player = null;

    protected bool firstTimeEntered = true;
    
    public int EnemyCount;
    public List<TriggerObj> trackedItems = new List<TriggerObj>();

    public void AddCharacter(Character character)
    {
        EnemyCount++;
        AddChild(character);
    }

    public void AddCharacter(string name, Vector2 globalPosition)
    {
        Character character = WorldUtilsSpawn.SpawnEnemy(this, globalPosition, name);
        if (character != null)
        {
            EnemyCount++;
        }
    }

    public void OnEnemyDeath()
    {
        EnemyCount--;
        if (EnemyCount <= 0) //can be -1 when there's no enemy at all
        {
            EnemyCount = 0;
            foreach (Node child in GetChildren())
            {
                if (child is Door door)
                {
                    door.RoomCleared();
                }
            }
        }
    }

    public void AddItem(Item item, Vector2 globalPosition)
    {
        TriggerObj obj = WorldUtilsSpawn.SpawnItem(this, globalPosition, item);
        if (obj != null)
        {
            trackedItems.Add(obj);
            //AddChild(obj);
        }
    }

    public void AddItemBase(Item item, Vector2 globalPosition)
    {
        TriggerObj obj = WorldUtilsSpawn.SpawnItemBase(this, globalPosition, item);
        if (obj != null)
        {
            trackedItems.Add(obj);
            //AddChild(obj);
        }
    }

    public override void _Ready()
    {
        base._Ready();
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

            foreach (Node child in GetChildren())
            {
                if (child is Door door)
                    door.FirstEnteredThisRoom(this);
            }

            if (EnemyCount == 0)
                OnEnemyDeath();
        }
    }

    public virtual void OnExitRoom()
    {
    }
    
    public static void DoorPosCalc(Node2D door, Vector2I LocalGrid, Vector2I LeadsTo)
    {
        door.Position = LocalGrid * WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        if (LeadsTo == Vector2I.Up)
        {
            door.Position += new Vector2(480/2, 42);
        }
        else if (LeadsTo == Vector2I.Down)
        {
            door.Position += new Vector2(480/2, 270 - 42);
            door.RotationDegrees = 180;
        }
        else if (LeadsTo == Vector2I.Left)
        {
            door.Position += new Vector2(66, 270/2);
            door.RotationDegrees = -90;
        }
        else if (LeadsTo == Vector2I.Right)
        {
            door.Position += new Vector2(480 - 66, 270/2);
            door.RotationDegrees = 90;
        }
    }
}