//Item(being triggered when player touches it)
//->CoinItem(s)
//->BombItem(s)
//->HeartItem(s)
//->KeyItem(s)
//->GrabBagItem
//->BatteryItem
//->EffectItem (passive items)
//->UsableItem (active items)
//->BuffItem (trinkets, affect status while held)
//->ConsumableItem (everything shown at the right-bottom corner of the screen, can be used once and gone)
//  ->Card
//  ->Capsule
//->... (blood machine, shopper item, etc.)

using Godot;
using System;

[GlobalClass]
public abstract partial class Item : Resource
{
    [Export] public string itemName = "Item";
    [Export] public string description = "This is an item.";
    [Export] public Texture2D icon;

    public abstract void OnPlayerGet(Player player);
    public abstract bool IsPickable(Player player);
}