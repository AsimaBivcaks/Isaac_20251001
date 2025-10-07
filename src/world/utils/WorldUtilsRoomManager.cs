using Godot;
using System;
using System.Collections.Generic;

//For Room-14 (_|), use its top-right corner as the reference point
public static class WorldUtilsRoomManager
{
    public const int MAX_ROOMS_X = 11;
    public const int MAX_ROOMS_Y = 11;

    public static Room CurrentRoom = null;
    public static Vector2I CurrentRoomGridPosition
    {
        get
        {
            if ( CurrentRoom == null ) return new Vector2I(-1,-1);
            return CurrentRoom.GridPosition;
        }
    }
    public static String[,] RoomArrangement = new String[MAX_ROOMS_X, MAX_ROOMS_Y];
    public static Vector2I[,] RoomArrangementOffsets = new Vector2I[MAX_ROOMS_X, MAX_ROOMS_Y]; //offsets for rooms that are larger than 1x1
    public static Room[,] Rooms = new Room[MAX_ROOMS_X, MAX_ROOMS_Y];
    public static Node RoomMount { set; private get; } = null;

    public static HashSet<Vector2I> LockedRooms = new HashSet<Vector2I>();
    public static HashSet<Vector2I> BossRooms = new HashSet<Vector2I>();

    public delegate void SR(Room originalRoom, Room newRoom);
    public static event SR RoomSwitched = new SR(OnSwitchRoom);
    //we don't care about SR when talking about generating&preparing the room

    public static bool CheckRoomAt(Vector2I gridPos)
    {
        if ( gridPos.X < 0 || gridPos.X >= MAX_ROOMS_X || gridPos.Y < 0 || gridPos.Y >= MAX_ROOMS_Y )
            return false;
        return !string.IsNullOrEmpty(RoomArrangement[gridPos.X, gridPos.Y]);
    }

    public static void AutoSetCurrentRoom(int x, int y)
    {
        AutoSetCurrentRoom(new Vector2I(x,y));
    }

    /*public static void AutoSetCurrentRoom(Vector2 playerGlobalPosition)
    {
        if ( RoomMount == null ) return;
        Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        int x = (int)(playerGlobalPosition.X / screenSize.X);
        int y = (int)(playerGlobalPosition.Y / screenSize.Y);
        if( x < 0 || x >= MAX_ROOMS_X || y < 0 || y >= MAX_ROOMS_Y )
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
    }*/

    public static void AutoSetCurrentRoom(Vector2I gridPos) //(x,y): anywhere in the room
    {
        if ( RoomMount == null ) return;
        //Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size");
        int x = gridPos.X;
        int y = gridPos.Y;
        if(string.IsNullOrEmpty(RoomArrangement[x,y]))
            return;
        if( IndexBoundCheck(x,y) )
            return;
        if( Rooms[x,y] == null )
        {
            TryLoadRoomAt(RoomArrangement[x,y], x, y);
        }
        SetCurrentRoom(x,y);
    }

    public static bool IndexBoundCheck(int x, int y)
    {
        return x < 0 || x >= MAX_ROOMS_X || y < 0 || y >= MAX_ROOMS_Y;
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

    public static bool TryArrangeRoomAt(string roomName, int x, int y) //(x,y): reference point
    {
        RoomSpace roomSpace = WorldUtilsPools.GetRoomSpace(roomName);
        if ( roomSpace == null )
        {
            GD.PrintErr("Failed to load RoomSpace for room ", roomName);
            return false;
        }

        if (roomSpace.Is14)
            return TryArrangeRoom14(roomName, x, y);
        
        uint code = RoomSpace.GetCode(roomSpace);
        if ( (code & 1) != 0 && (IndexBoundCheck(x,y) || !string.IsNullOrEmpty(RoomArrangement[x,y])) )
            return false;
        if ( (code & 2) != 0 && (IndexBoundCheck(x+1,y) || !string.IsNullOrEmpty(RoomArrangement[x+1,y])) )
            return false;
        if ( (code & 4) != 0 && (IndexBoundCheck(x,y+1) || !string.IsNullOrEmpty(RoomArrangement[x,y+1])) )
            return false;
        if ( (code & 8) != 0 && (IndexBoundCheck(x+1,y+1) || !string.IsNullOrEmpty(RoomArrangement[x+1,y+1])) )
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
    private static bool TryArrangeRoom14(string roomName, int x, int y)
    {
        if ( IndexBoundCheck(x,y) || !string.IsNullOrEmpty(RoomArrangement[x,y]) )
            return false;
        if ( IndexBoundCheck(x,y+1) || !string.IsNullOrEmpty(RoomArrangement[x,y+1]) )
            return false;
        if ( IndexBoundCheck(x-1,y+1) || !string.IsNullOrEmpty(RoomArrangement[x-1,y+1]) )
            return false;
        
        RoomArrangement[x,y] = roomName;
        RoomArrangementOffsets[x,y] = new Vector2I(0,0);
        RoomArrangement[x,y+1] = roomName;
        RoomArrangementOffsets[x,y+1] = new Vector2I(0,1);
        RoomArrangement[x-1,y+1] = roomName;
        RoomArrangementOffsets[x-1,y+1] = new Vector2I(-1,1);

        return true;
    }

    public static bool TryLoadRoomAt(string roomName, int x, int y) //(x,y): anywhere in the room
    {
        RoomSpace roomSpace = WorldUtilsPools.GetRoomSpace(roomName);
        if ( roomSpace == null ) return false;

        uint code = RoomSpace.GetCode(roomSpace);
        Room room = WorldUtilsPools.GetResource<PackedScene>(roomName)?.Instantiate<Room>();
        if( room == null ) return false;
        Vector2I offset = RoomArrangementOffsets[x,y];
        x -= offset.X;
        y -= offset.Y;

        room.GridPosition = new Vector2I(x,y);
        if (roomSpace.Is14)
            return LoadRoom14(room, x, y);

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
    private static bool LoadRoom14(Room room, int x, int y)
    {
        Rooms[x,y] = room;
        Rooms[x,y+1] = room;
        Rooms[x-1,y+1] = room;
        return true;
    }

    public static void SetCurrentRoom(int x, int y)
    {
        if ( x < 0 || x >= MAX_ROOMS_X || y < 0 || y >= MAX_ROOMS_Y )
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
        for ( int i = 0; i < MAX_ROOMS_X; i++ )
            for ( int j = 0; j < MAX_ROOMS_Y; j++ )
                if( Rooms[i,j] != null && (Rooms[i,j] != CurrentRoom || forced) )
                {
                    Rooms[i,j].QueueFree();
                    Rooms[i,j] = null;
                }
    }

    public static void LockRoomAt(Vector2I gridPos, uint roomCode)
    {
        if ( CheckRoomAt(gridPos) && roomCode != 14 )
        {
            if ((roomCode & 1) != 0 && !LockedRooms.Contains(gridPos))
            {
                LockedRooms.Add(gridPos);
            }
            if ((roomCode & 2) != 0 && !LockedRooms.Contains(gridPos + Vector2I.Right))
            {
                LockedRooms.Add(gridPos + Vector2I.Right);
            }
            if ((roomCode & 4) != 0 && !LockedRooms.Contains(gridPos + Vector2I.Down))
            {
                LockedRooms.Add(gridPos + Vector2I.Down);
            }
            if ((roomCode & 8) != 0 && !LockedRooms.Contains(gridPos + Vector2I.Right + Vector2I.Down))
            {
                LockedRooms.Add(gridPos + Vector2I.Right + Vector2I.Down);
            }
        }
        else if ( roomCode == 14 )
        {
            if ( CheckRoomAt(gridPos) && !LockedRooms.Contains(gridPos) )
                LockedRooms.Add(gridPos);
            if ( CheckRoomAt(gridPos + Vector2I.Down) && !LockedRooms.Contains(gridPos + Vector2I.Down) )
                LockedRooms.Add(gridPos + Vector2I.Down);
            if ( CheckRoomAt(gridPos + Vector2I.Left + Vector2I.Down) && !LockedRooms.Contains(gridPos + Vector2I.Left + Vector2I.Down) )
                LockedRooms.Add(gridPos + Vector2I.Left + Vector2I.Down);
        }
        
    }

    private static void OnSwitchRoom(Room r1, Room r2)
    {
    }
}