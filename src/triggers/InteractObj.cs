using Godot;
using System;

public partial class InteractableObj : TriggerObj
{
    public virtual void OnPlayerInteract(Player player)
    {
        GD.Print("Interacted.");
    }
}