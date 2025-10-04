using Godot;
using System;

public class RedFlyMoveBehavior : CharacterBehavior
{
    private float velocityRefValue;

    private Player player;

    public RedFlyMoveBehavior(RedFly _self, float _velocityRefValue) : base(_self)
    {
        velocityRefValue = _velocityRefValue;
        player = WorldUtilsBlackboard.Get<Player>("player_instance");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (statusF["pause"] > .5f) return;

        Vector2 direction = player.GlobalPosition - self.GlobalPosition;
        if(direction.LengthSquared() > 4f)
            direction = direction.Normalized();
        else
            direction = new Vector2(0,0);
        self.Velocity = direction * velocityRefValue;

        self.Move(delta);
    }
}