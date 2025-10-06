using Godot;
using System;

public partial class Camera : Camera2D
{
    public Character Follow;
    private Vector2 halfScreenSize;

    public override void _Ready()
    {
        base._Ready();
        halfScreenSize = WorldUtilsBlackboard.Get<Vector2I>("screen_size") / 2;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Follow != null)
        {
            GlobalPosition = Follow.GlobalPosition;
            if (WorldUtilsRoomManager.CurrentRoom != null)
            {
                GlobalPosition = WorldUtilsRoomManager
                    .CurrentRoom
                    .roomSpace
                    .GetIdealCameraCenterPosition(Follow.GlobalPosition)
                - halfScreenSize;
            }
        }
        else
        {
            Follow = WorldUtilsBlackboard.Get<Player>("player_instance");
        }
    }
}