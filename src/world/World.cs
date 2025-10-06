using Godot;
using System;

public partial class World : Node2D
{
    public static World Instance { get; private set; } = null;

    public override void _Ready()
    {
        base._Ready();
        if ( Instance != null )
        {
            GD.PrintErr("Error: multiple World instances.");
            QueueFree();
            return;
        }
        Instance = this;
    }

    public void IntoDoor(Vector2I doorLocalGrid, Vector2I doorLeadsTo)
    {
        CallDeferred("IntoDoorInternal", doorLocalGrid, doorLeadsTo);
    }

    public void IntoDoorInternal(Vector2I doorLocalGrid, Vector2I doorLeadsTo)
    {
        if ( WorldUtilsRoomManager.CurrentRoom == null )
        {
            GD.PrintErr("Used door before the room is ready.");
            return;
        }

        Vector2I targetRoomPos = WorldUtilsRoomManager.CurrentRoom.GridPosition + doorLeadsTo;
        if ( !WorldUtilsRoomManager.CheckRoomAt(targetRoomPos) )
        {
            GD.PrintErr("Target room is not valid.");
            return;
        }

        WorldUtilsRoomManager.AutoSetCurrentRoom(targetRoomPos);
        if ( WorldUtilsRoomManager.CurrentRoom == null )
        {
            GD.PrintErr("Failed to switch to target room.");
            return;
        }

        Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        Vector2I localGrid = targetRoomPos - WorldUtilsRoomManager.CurrentRoom.GridPosition;
        Vector2 newPos = localGrid * screenSize;
        if (doorLeadsTo == Vector2I.Down)
        {
            newPos += new Vector2(480/2, 42 + 22);
        }
        else if (doorLeadsTo == Vector2I.Up)
        {
            newPos += new Vector2(480/2, 270 - 42 - 22);
        }
        else if (doorLeadsTo == Vector2I.Right)
        {
            newPos += new Vector2(66 + 16, 270/2);
        }
        else if (doorLeadsTo == Vector2I.Left)
        {
            newPos += new Vector2(480 - 66 - 16, 270/2);
        }
        
        var player = WorldUtilsBlackboard.Get<Player>("player_instance");
        if ( player != null )
            player.GlobalPosition = newPos;
    }
}
