using Godot;
using System;

public partial class RoomSpace : Node2D
{
    [Export] public Vector2I RoomGridPosition = new Vector2I(0,0);//left-top
    [Export] public Vector2I RoomSizeScreens = new Vector2I(1,1);
    [Export] public bool RoomGap; //L-shaped rooms
    [Export] public Vector2I RoomGapPosition = new Vector2I(0,0);

    private Vector2 screenSize;
    private Vector2 halfScreenSize;

    public override void _Ready()
    {
        base._Ready();

        screenSize = WorldUtilsBlackboard.Get<Vector2>("screen_size");
        halfScreenSize = screenSize * 0.5f;
    }

    public Vector2 GetIdealCameraCenterPosition(Vector2 playerGlobalPosition)
    {
        Vector2 playerRoomPos = playerGlobalPosition - GlobalPosition;
        Vector2 roomSize = RoomSizeScreens * screenSize;

        Vector2 res = new Vector2(
            Mathf.Clamp(playerRoomPos.X, halfScreenSize.X, roomSize.X - halfScreenSize.X),
            Mathf.Clamp(playerRoomPos.Y, halfScreenSize.Y, roomSize.Y - halfScreenSize.Y)
        );

        return res + GlobalPosition;
    }
}