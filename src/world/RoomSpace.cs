using Godot;
using System;

[GlobalClass]
public partial class RoomSpace : Resource
{
    [Export] public Vector2I RoomSizeScreens = new Vector2I(1,1);
    [Export] public bool RoomGap = false; //L-shaped rooms
    [Export] public Vector2I RoomGapPosition = new Vector2I(0,0);

    public static uint GetCode(RoomSpace rs)
    {
        uint code = 1;
        code |= rs.RoomSizeScreens.X == 2 ? 2u : 0u;
        code |= rs.RoomSizeScreens.Y == 2 ? 4u : 0u;
        code |= (rs.RoomSizeScreens.X==2 && rs.RoomSizeScreens.Y==2) ? 8u : 0u;
        if (rs.RoomGap)
        {
            if( rs.RoomGapPosition == new Vector2I(0,0) ) code &= ~1u;
            else if( rs.RoomGapPosition == new Vector2I(1,0) ) code &= ~2u;
            else if( rs.RoomGapPosition == new Vector2I(0,1) ) code &= ~4u;
            else if( rs.RoomGapPosition == new Vector2I(1,1) ) code &= ~8u;
        }
        return code;
    }

    public Vector2 GetIdealCameraCenterPosition(Vector2 playerGlobalPosition)
    {
        Vector2 playerRoomPos = playerGlobalPosition;
        Vector2 roomSize = RoomSizeScreens * new Vector2I( 480, 270 );

        Vector2 res = new Vector2(
            Mathf.Clamp(playerRoomPos.X, 240, roomSize.X - 240),
            Mathf.Clamp(playerRoomPos.Y, 135, roomSize.Y - 135)
        );

        return res;
    }
}