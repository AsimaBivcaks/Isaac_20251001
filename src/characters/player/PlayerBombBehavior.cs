using Godot;
using System;
using System.Reflection.Metadata;

public class PlayerBombBehavior : CharacterBehavior
{
    private const int MAX_BOMBS = 99;

    private float bombCDTimer; //current cooldown
    [Export] public float BombCD = .6f;

    private int bombs = 3; //current bombs

    public int Bombs{ //This was based on statusF["bombs"], however now it's pretty much something useless
        get{
            return bombs;
        }
        private set{
            bombs = Math.Clamp(value, 0, MAX_BOMBS);
        }
    }

    public PlayerBombBehavior(Player _self) : base(_self)
    {
    }

    public override void PlugIn()
    {
        base.PlugIn();
        bombCDTimer = 0.0f;
    }

    public override void UnPlug()
    {
        base.UnPlug();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if(bombCDTimer > 0.0f)
            bombCDTimer -= (float)delta;

        if(self.statusF["pause"] > .5f) return;
        
        if(Input.IsActionJustPressed("bomb"))
            UseBomb();
    }

    public void UseBomb()
    {
        if(bombCDTimer >= .01f || Bombs <= 0) return;
        bombCDTimer = BombCD;
        Bombs--;
        //generate a BombObj at player's position
        WorldUtilsSpawn.SpawnBomb(self.Position + new Vector2(0, -.5f), self.Mount);
    }

    public void AddBomb(int amount=1)
    {
        Bombs += amount;
    }

    public bool CanAddBomb()
    {
        return Bombs < MAX_BOMBS;
    }
}