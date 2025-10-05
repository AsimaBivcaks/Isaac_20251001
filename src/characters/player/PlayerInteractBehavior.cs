using Godot;
using System;

public class PlayerInteractBehavior : CharacterBehavior
{
    private Area2D interactArea;

    //private PlayerEffectManagementBehavior effectBehavior;
    //private PlayerUsableManagementBehavior usableBehavior;

    public PlayerInteractBehavior(Character _character, Area2D _interactArea) : base(_character)
    {
        interactArea = _interactArea;
    }

    public override void _Ready()
    {
        base._Ready();
        interactArea.Monitoring = true;
        interactArea.CollisionMask = 4;
        interactArea.CollisionLayer = 0;
        interactArea.AreaEntered += (Area2D body) => {
            if(body is ItemObj obj){
                if(obj.Get(self as Player)){
                    obj.Destroy();
                }
                else
                {
                    obj.Push((obj.GlobalPosition - self.GlobalPosition).Normalized(), 40.0f);
                }
            }
            if(body is InteractableObj obj2){
                obj2.OnPlayerInteract((Player)self);
            }
        };
    }

    /*public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("interact"))
        {
            foreach(Node2D body in interactArea.GetOverlappingBodies())
            {
                if(body is InteractableObj obj)
                {
                    obj.OnPlayerInteract((Player)self);
                }
            }
        }
    }*/
}