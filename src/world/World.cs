using Godot;
using System;

public partial class World : Node2D
{
    [Export] SpawnPool roomPool;

    public static World Instance { get; private set; } = null;

    private WorldGenerator generator = new WorldGenerator();

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

        WorldUtilsRandom.Init((int)(Time.GetUnixTimeFromSystem() * 10000000007));
        WorldUtilsRng.Init((int)(Time.GetUnixTimeFromSystem() * 10000000007 * 31));
        WorldUtilsRoomManager.RoomMount = this;
        generator.roomPool = roomPool;

        Vector2I startRoom = new Vector2I(5,5);
        generator.GenerateWorldFrom(startRoom);

        CallDeferred("EndReady", startRoom);
    }

    public void EndReady(Vector2I startRoom)
    {
        PackedScene playerScene = WorldUtilsPools.GetResource<PackedScene>("player");
        if ( playerScene == null ) return;
        var player = playerScene.Instantiate<Player>();
        if ( player == null ) return;
        player.Mount = this;
        player.GlobalPosition = new Vector2(84, 87);
        AddChild(player);

        PackedScene hudScene = WorldUtilsPools.GetResource<PackedScene>("hud");
        if ( hudScene == null ) return;
        var hud = hudScene.Instantiate<CanvasLayer>();
        if ( hud == null ) return;
        GetParent().AddChild(hud);

        WorldUtilsRoomManager.AutoSetCurrentRoom(startRoom);
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

        Vector2I targetRoomPos = WorldUtilsRoomManager.CurrentRoom.GridPosition + doorLocalGrid + doorLeadsTo;
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
        if (WorldUtilsRoomManager.CurrentRoom.roomSpace.Is14)
            newPos += new Vector2(480, 0);
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
