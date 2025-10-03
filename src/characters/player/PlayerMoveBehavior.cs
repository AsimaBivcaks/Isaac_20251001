using Godot;
using System;

public class PlayerMoveBehavior : CharacterBehavior
{
    private float moveAcceleration;
    private float velocityRefValue;

    PlayerAnimationController anim;

    public PlayerMoveBehavior(Player _self, float _moveAcceleration, float _velocityRefValue) : base(_self)
    {
        moveAcceleration = _moveAcceleration;
        velocityRefValue = _velocityRefValue;
    }

    public override void _Ready()
    {
        base._Ready();
        
        statusV["facing"] = new Vector2(0,1);
        statusV["move"] = new Vector2();
        statusV["forced_facing"] = new Vector2(0,0);

        anim = ((Player)self).anim;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        float vlen = self.Velocity.Length();
        if (vlen < 0.1f) vlen = 0.1f; //avoid NaN
        anim.SetBody(self.Velocity/vlen);
        if(statusV["forced_facing"] != Vector2.Zero)
            anim.SetHead(statusV["forced_facing"].Normalized(), 255f, statusF["emitted_this_frame"] > 0.5f);
        else
            anim.SetHead(self.Velocity/vlen, vlen, statusF["emitted_this_frame"] > 0.5f);

        Vector2 facing = statusV["facing"];
        Vector2 moveDir = statusV["move"];

        //opposite inputs cancel out each other
        Vector2 dir = new Vector2(
            Input.GetAxis("m_left","m_right"),
            Input.GetAxis("m_up","m_down")
        );

        if (dir != Vector2.Zero)
            facing = dir;
        else
            facing = new Vector2(0,1);
        
        if (facing.Y != 0) facing.X = 0; //vertical inputs over horizontal ones

        if (dir == Vector2.Zero)
            moveDir = Vector2.Zero;
        else
            moveDir = facing;

        dir = dir.Normalized() * velocityRefValue;
        float acc = moveAcceleration;
        if (dir.X * self.Velocity.X < -10f)
            acc *= 5;
        if (dir.Y * self.Velocity.Y < -10f)
            acc *= 5;
        self.Velocity = self.Velocity.MoveToward(dir, (float)(delta * acc));

        statusV["facing"] = facing;
        statusV["move"] = moveDir;

        self.Move(delta);
    }
}
