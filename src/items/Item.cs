//Item(being triggered when player touches it)
//->Coin
//->Bomb
//->Heart
//->KeyItem
//->GrabBag
//->Battery
//->EffectItem (passive items)
//->UsableItem (active items)
//->BuffItem (trinkets, affect status while held)
//->ConsumableItem (everything shown at the right-bottom corner of the screen, can be used once and gone)
//  ->Card
//  ->Capsule
//->... (blood machine, shopper item, etc.)

//TriggerObj
//->ItemObj
//->InteractableObj
//->BombObj (item=null)

using Godot;
using System;

public abstract partial class Item : Resource
{
    [Export] public string itemName = "Item";
    [Export] public string description = "This is an item.";
    [Export] public Texture2D icon;

    public abstract void OnTrigger(Player player);
}