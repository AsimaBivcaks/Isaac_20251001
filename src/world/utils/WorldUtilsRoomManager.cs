using Godot;
using System;

public static class WorldUtilsRoomManager
{
    public static Room CurrentRoom = null;
    public static Vector2I CurrentRoomGridPosition
    {
        get
        {
            if ( CurrentRoom == null ) return new Vector2I(-1,-1);
            return CurrentRoom.GridPosition;
        }
    }
    private static String[,] RoomArrangement = new String[10,10]; //max 10x10 screens
    private static Vector2I[,] RoomArrangementOffsets = new Vector2I[10,10]; //offsets for rooms that are larger than 1x1
    private static Room[,] Rooms = new Room[10,10];
    public static Node RoomMount { set; private get; } = null;

    public delegate void SR(Room originalRoom, Room newRoom);
    public static event SR RoomSwitched = new SR(OnSwitchRoom);
    //we don't care about SR when talking about generating&preparing the room

    public static bool CheckRoomAt(Vector2I gridPos)
    {
        if ( gridPos.X < 0 || gridPos.X >= Rooms.GetLength(0) || gridPos.Y < 0 || gridPos.Y >= Rooms.GetLength(1) )
            return false;
        return !string.IsNullOrEmpty(RoomArrangement[gridPos.X, gridPos.Y]);
    }

    public static void AutoSetCurrentRoom(int x, int y)
    {
        AutoSetCurrentRoom(new Vector2I(x,y));
    }

    public static void AutoSetCurrentRoom(Vector2 playerGlobalPosition)
    {
        if ( RoomMount == null ) return;
        Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        int x = (int)(playerGlobalPosition.X / screenSize.X);
        int y = (int)(playerGlobalPosition.Y / screenSize.Y);
        if( x < 0 || x >= Rooms.GetLength(0) || y < 0 || y >= Rooms.GetLength(1) )
            return;
        if( Rooms[x,y] == null )
        {
            var offset = RoomArrangementOffsets[x,y];
            x -= offset.X;
            y -= offset.Y;
            if(!TryLoadRoomAt(RoomArrangement[x,y], x, y))
            {
                GD.PrintErr("Failed to load room at ", x, ",", y," due to spacial overlapping or other issues.");
                return;
            }
        }
        SetCurrentRoom(x,y);
    }

    public static void AutoSetCurrentRoom(Vector2I gridPos)
    {
        if ( RoomMount == null ) return;
        //Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        int x = gridPos.X;
        int y = gridPos.Y;
        if( x < 0 || x >= Rooms.GetLength(0) || y < 0 || y >= Rooms.GetLength(1) )
            return;
        if( Rooms[x,y] == null )
        {
            if(!TryLoadRoomAt(RoomArrangement[x,y], x, y))
            {
                GD.PrintErr("Failed to load room at ", x, ",", y," due to spacial overlapping or other issues.");
                return;
            }
        }
        SetCurrentRoom(x,y);
    }

    /*public static bool TryLoadRoomAt(string roomName, int x, int y)
    {
        RoomSpace roomSpace = WorldUtilsPools.GetRoomSpace(roomName);
        if ( roomSpace == null )
        {
            GD.PrintErr("Failed to load RoomSpace for room ", roomName);
            return false;
        }

        if ( !roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,0) )
        {
            if( !string.IsNullOrEmpty(RoomArrangement[x,y]) )
                return false;
        }
        if ( roomSpace.RoomSizeScreens.X == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,0)) )
        {
            if( !string.IsNullOrEmpty(RoomArrangement[x+1,y]) )
                return false;
        }
        if ( roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,1)) )
        {
            if( !string.IsNullOrEmpty(RoomArrangement[x,y+1]) )
                return false;
        }
        if ( roomSpace.RoomSizeScreens.X == 2 && roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,1)) )
        {
            if( !string.IsNullOrEmpty(RoomArrangement[x+1,y+1]) )
                return false;
        }

        Room room = WorldUtilsPools.GetResource<PackedScene>(roomName)?.Instantiate<Room>();
        if( room == null )
        {
            GD.PrintErr("Failed to load Room for room ", roomName);
            return false;
        }
        room.GridPosition = new Vector2I(x,y);

        if ( !roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,0) )
            Rooms[x,y] = room;
        if ( roomSpace.RoomSizeScreens.X == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,0)) )
            Rooms[x+1,y] = room;
        if ( roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,1)) )
            Rooms[x,y+1] = room;
        if ( roomSpace.RoomSizeScreens.X == 2 && roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,1)) )
            Rooms[x+1,y+1] = room;
        //damn this is so messed up
        return true;
    }*/

    public static bool TryArrangeRoomAt(string roomName, int x, int y)
    {
        RoomSpace roomSpace = WorldUtilsPools.GetRoomSpace(roomName);
        if ( roomSpace == null )
        {
            GD.PrintErr("Failed to load RoomSpace for room ", roomName);
            return false;
        }
        uint code = RoomSpace.GetCode(roomSpace);
        if ( (code & 1) != 0 && !string.IsNullOrEmpty(RoomArrangement[x,y]) )
            return false;
        if ( (code & 2) != 0 && !string.IsNullOrEmpty(RoomArrangement[x+1,y]) )
            return false;
        if ( (code & 4) != 0 && !string.IsNullOrEmpty(RoomArrangement[x,y+1]) )
            return false;
        if ( (code & 8) != 0 && !string.IsNullOrEmpty(RoomArrangement[x+1,y+1]) )
            return false;
        
        if ( (code & 1) != 0 )
            RoomArrangement[x,y] = roomName;
        if ( (code & 2) != 0 )
        {
            RoomArrangement[x+1,y] = roomName;
            RoomArrangementOffsets[x+1,y] = new Vector2I(1,0);
        }
        if ( (code & 4) != 0 )
        {
            RoomArrangement[x,y+1] = roomName;
            RoomArrangementOffsets[x,y+1] = new Vector2I(0,1);
        }
        if ( (code & 8) != 0 )
        {
            RoomArrangement[x+1,y+1] = roomName;
            RoomArrangementOffsets[x+1,y+1] = new Vector2I(1,1);
        }

        return true;
    }

    public static bool TryLoadRoomAt(string roomName, int x, int y)
    {
        RoomSpace roomSpace = WorldUtilsPools.GetRoomSpace(roomName);
        if ( roomSpace == null )
        {
            GD.PrintErr("Failed to load RoomSpace for room ", roomName);
            return false;
        }

        uint code = RoomSpace.GetCode(roomSpace);
        GD.Print("Room code: ", code);
        if ( (code & 1) != 0 && Rooms[x,y] != null )
            return false;
        if ( (code & 2) != 0 && Rooms[x+1,y] != null )
            return false;
        if ( (code & 4) != 0 && Rooms[x,y+1] != null )
            return false;
        if ( (code & 8) != 0 && Rooms[x+1,y+1] != null )
            return false;
        GD.Print("Space available for room ", roomName);
        Room room = WorldUtilsPools.GetResource<PackedScene>(roomName)?.Instantiate<Room>();
        if( room == null )
        {
            GD.PrintErr("Failed to load Room for room ", roomName);
            return false;
        }
        room.GridPosition = new Vector2I(x,y);

        if ( (code & 1) != 0 )
            Rooms[x,y] = room;
        if ( (code & 2) != 0 )
            Rooms[x+1,y] = room;
        if ( (code & 4) != 0 )
            Rooms[x,y+1] = room;
        if ( (code & 8) != 0 )
            Rooms[x+1,y+1] = room;
        
        return true;
    }

    public static void SetCurrentRoom(int x, int y)
    {
        if ( x < 0 || x >= Rooms.GetLength(0) || y < 0 || y >= Rooms.GetLength(1) )
            return;
        Room room = Rooms[x,y];
        if ( room == null || room == CurrentRoom || RoomMount == null )
            return;
        Room originalRoom = CurrentRoom;
        if ( CurrentRoom != null )
        {
            CurrentRoom.OnExitRoom();
            RoomMount.RemoveChild(CurrentRoom);
        }
        RoomMount.AddChild(room);
        CurrentRoom = room;
        CurrentRoom.OnEnterRoom();
        RoomSwitched(originalRoom, room);
    }

    public static void ClearLoadedRooms(bool forced = false)
    {
        for ( int i = 0; i < Rooms.GetLength(0); i++ )
            for ( int j = 0; j < Rooms.GetLength(1); j++ )
                if( Rooms[i,j] != null && (Rooms[i,j] != CurrentRoom || forced) )
                {
                    Rooms[i,j].QueueFree();
                    Rooms[i,j] = null;
                }
    }

    private static void OnSwitchRoom(Room r1, Room r2)
    {
    }
}