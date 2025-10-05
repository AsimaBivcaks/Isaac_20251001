using Godot;
using System;

public static class WorldUtilsRoomManager
{
    public static Room CurrentRoom = null;
    public static string[,] RoomArrangement = new string[10,10]; //max 10x10 screens
    public static Room[,] Rooms = new Room[10,10];
    public static Node RoomMount { set; private get; } = null;

    public static void AutoSetCurrentRoom(Vector2 playerGlobalPosition)
    {
        if ( RoomMount == null ) return;
        Vector2 screenSize = WorldUtilsBlackboard.Get<Vector2>("screen_size");
        int x = (int)(playerGlobalPosition.X / screenSize.X);
        int y = (int)(playerGlobalPosition.Y / screenSize.Y);
        if( x < 0 || x >= Rooms.GetLength(0) || y < 0 || y >= Rooms.GetLength(1) )
            return;
        if( Rooms[x,y] == null )
            LoadRoomAt(RoomArrangement[x,y], x, y);
        SetCurrentRoom(x,y);
    }

    public static void LoadRoomAt(string roomName, int x, int y)
    {
        Room room = WorldUtilsPools.GetResource<PackedScene>(roomName)?.Instantiate<Room>();
        if( room == null ) return;
        RoomSpace roomSpace = room.GetNode<RoomSpace>("RoomSpace");
        if ( roomSpace == null ) return;
        roomSpace.RoomGridPosition = new Vector2I(x,y);

        if ( !roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,0) )
        {
            if( Rooms[x,y] != null )
            {
                GD.PrintErr("Room overlap at (" + x + "," + y + ")");
                return;
            }
            Rooms[x,y] = room;
        }
        if ( roomSpace.RoomSizeScreens.X == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,0)) )
        {
            Rooms[x+1,y] = room;
            if( Rooms[x+1,y] != null )
            {
                GD.PrintErr("Room overlap at (" + (x+1) + "," + y + ")");
                return;
            }
        }
        if ( roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(0,1)) )
        {
            Rooms[x,y+1] = room;
            if( Rooms[x,y+1] != null )
            {
                GD.PrintErr("Room overlap at (" + x + "," + (y+1) + ")");
                return;
            }
        }
        if ( roomSpace.RoomSizeScreens.X == 2 && roomSpace.RoomSizeScreens.Y == 2 && (!roomSpace.RoomGap || roomSpace.RoomGapPosition != new Vector2I(1,1)) )
        {
            Rooms[x+1,y+1] = room;
            if( Rooms[x+1,y+1] != null )
            {
                GD.PrintErr("Room overlap at (" + (x+1) + "," + (y+1) + ")");
                return;
            }
        }
    }

    public static void SetCurrentRoom(int x, int y)
    {
        if ( x < 0 || x >= Rooms.GetLength(0) || y < 0 || y >= Rooms.GetLength(1) )
            return;
        Room room = Rooms[x,y];
        if ( room == null || room == CurrentRoom || RoomMount == null )
            return;
        if ( CurrentRoom != null )
        {
            CurrentRoom.OnExitRoom();
            RoomMount.RemoveChild(CurrentRoom);
        }
        RoomMount.AddChild(room);
        CurrentRoom = room;
        CurrentRoom.OnEnterRoom();
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
}