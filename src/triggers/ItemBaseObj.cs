using Godot;
using System;

public partial class ItemBaseObj : InteractableObj
{
    [Export] public Item item;

    private Sprite2D sprite;
    
    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite2D>("Spr");
        sprite.Texture = item.Icon;
    }

    public override void OnPlayerInteract(Player player)
    {
        item?.OnPlayerGet(player);
        sprite.Visible = false;
        item = null;
    }
}