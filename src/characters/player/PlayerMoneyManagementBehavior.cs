using Godot;
using System;

public class PlayerMoneyManagementBehavior : CharacterBehavior
{
    public int Money { get; private set; }

    public PlayerMoneyManagementBehavior(Player _self) : base(_self)
    {
    }

    public override void _Ready()
    {
        base._Ready();

        Money = 0;
    }

    public void AddMoney(int amount = 1)
    {
        Money += amount;
    }

    public bool UseMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            return true;
        }
        return false;
    }
}