using Godot;
using System;

public abstract class CharacterHPBehavior : CharacterBehavior
{
    protected const float REDBLINK_DURATION = 0.15f;

    private Tween redBlink = null;

    public int MaxHP { get; protected set; }
    public int HP { get; protected set; }

    public Callable? DeathCallback;

    public CharacterHPBehavior(Character _self, int _maxHP, Callable? deathCallback=null) : base(_self)
    {
        MaxHP = _maxHP;
        HP = MaxHP;
        DeathCallback = deathCallback;
    }

    public override void _Ready()
    {
        base._Ready();
        HP = MaxHP;
    }

    protected abstract bool ProcessDamage(DamageData damageData); //return true if actual damage is applied

    public void TakeDamage(DamageData damageData)
    {
        if (self.statusF["pause"] > 0.5f) return;
        if (ProcessDamage(damageData))
        {
            if(redBlink != null)
            {
                redBlink.Kill();
            }
            if (HP <= 0)
            {
                DeathCallback?.Call();
                return;
            }
            redBlink = self.GetTree().CreateTween();
            self.Modulate = new Color(1, 0.5f, 0.5f);
            redBlink.TweenCallback(Callable.From(() => {
                    self.Modulate = Colors.White;
            })).SetDelay(REDBLINK_DURATION);
        }
    }
}
