using Godot;
using System;

public partial class InteractableObj : TriggerObj
{
    public virtual void OnPlayerInteract(Player player)
    {
        //DEBUG
        GD.Print("Interacted with object: ",this.GetType());
    }
}