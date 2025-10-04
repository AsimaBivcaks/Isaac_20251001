using Godot;
using System;

public class PlayerKeyManagementBehavior : CharacterBehavior
{
    public int Keys { get; private set; }

    public PlayerKeyManagementBehavior(Player _self) : base(_self)
    {
    }

    public override void _Ready()
    {
        base._Ready();

        Keys = 0;
    }

    public void AddKey(int amount = 1)
    {
        Keys += amount;
    }

    public bool UseKey()
    {
        if (Keys > 0)
        {
            Keys--;
            return true;
        }
        return false;
    }
}