using Godot;
using System;
using System.Reflection.Metadata;

public class PlayerBombBehavior : CharacterBehavior
{
    private const int MAX_BOMBS = 99;

    private float bombCDTimer; //current cooldown
    [Export] public float BombCD = .6f;

    public int Bombs{
        get{
            if(!self.statusF.ContainsKey("bombs"))
                self.statusF["bombs"] = 0;
            return (int)(self.statusF["bombs"] + .01f); //idk if this is really necessary
        }
        set{
            self.statusF["bombs"] = Math.Clamp(value, 0, MAX_BOMBS);
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
        
        if(Input.IsActionJustPressed("bomb"))
            UseBomb();
    }

    public void UseBomb()
    {
        if(bombCDTimer >= .01f || Bombs <= 0) return;
        bombCDTimer = BombCD;
        Bombs--;
        //generate a BombObj at player's position
        WorldUtilsTriggers.SpawnBomb(self.Position, self.Mount);
    }
}