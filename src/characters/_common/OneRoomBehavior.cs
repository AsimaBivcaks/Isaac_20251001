using Godot;
using System;

public abstract class OneRoomBehavior : CharacterBehavior
{
    protected OneRoomBehavior(Character _self) : base(_self)
    {
    }

    public override void PlugIn()
    {
        base.PlugIn();
        WorldUtilsRoomManager.RoomSwitched += OnExitRoom;
    }

    protected void OnExitRoom(Room r1, Room r2)
    {
        self.RemoveBehavior(this);
    }

    public override void UnPlug()
    {
        base.UnPlug();
        WorldUtilsRoomManager.RoomSwitched -= OnExitRoom;
    }
}