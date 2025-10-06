using Godot;
using System;

public class PlayerUsableManagementBehavior : CharacterBehavior
{
    //TODO: CD time & Use (in _Process)
    private const double USE_CD = 0.5;

    public UsableItem item { get; private set; } = null;
    public int Energy { get; private set; } = 0;
    public int MaxEnergy { get; private set; } = 0;

    private double useCDTimer = 0;

    public PlayerUsableManagementBehavior(Player _self) : base(_self)
    {
    }

    public override void _Ready()
    {
        base._Ready();

        item = null;
    }

    public bool CanFillEnergy()
    {
        return item != null && Energy < MaxEnergy;
    }

    public void FillEnergy(int amount)
    {
        if (item != null)
        {
            Energy += amount;
            if (Energy > MaxEnergy) Energy = MaxEnergy;
        }
    }

    public bool TryUseEnergy(int amount=1)
    {
        if (Energy < amount) return false;
        Energy -= amount;
        return true;
    }

    public void SetItem(UsableItem newItem)
    {
        if (item != null)
            WorldUtilsSpawn.SpawnItem(self.Mount, self.Position, item, false);

        item = newItem;
        if (MaxEnergy == 0)
        {
            MaxEnergy = newItem.MaxEnergy;
            Energy = MaxEnergy;
        }
        else
        {
            Energy = (int)( (float)newItem.MaxEnergy * Energy / MaxEnergy + .5f );
            MaxEnergy = newItem.MaxEnergy;
            if (Energy > MaxEnergy) Energy = MaxEnergy;
        }
    }

    public bool UseItem(Player player)
    {
        if (item != null)
        {
            item.OnUse(player);
            item = null;
            return true;
        }
        return false;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (useCDTimer >= 0)
            useCDTimer -= delta;
        else
        {
            if(Input.IsActionJustPressed("interact") && item != null)
            {
                item.OnUse((Player)self);
            }
        }
    }
}