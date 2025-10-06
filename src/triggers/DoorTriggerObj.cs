using Godot;
using System;

public partial class DoorTriggerObj : InteractableObj
{
    public Callable OnEnter;

    public override void OnPlayerInteract(Player player)
    {
        base.OnPlayerInteract(player);
        OnEnter.Call();
    }
}