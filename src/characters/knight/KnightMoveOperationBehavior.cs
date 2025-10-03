using Godot;
using System;

public class KnightMoveOperationBehavior : CharacterBehavior
{
    private float velocityRefValue1;
    private float velocityRefValue2;
    private float moveAcceleration;

    public KnightMoveOperationBehavior(Character _self, float _velocityRefValue1, float _velocityRefValue2, float _moveAcceleration) : base(_self)
    {
        velocityRefValue1 = _velocityRefValue1;
        velocityRefValue2 = _velocityRefValue2;
        moveAcceleration = _moveAcceleration;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        Vector2 dir = statusV["input"];
        if(dir.Length() > .1f)
        {
            if(statusF["dash"] < 0.5f)
            {
                statusV["facing"] = self.Velocity.Normalized();
            }
            else statusV["facing"] = dir;
        }
        else
        {
            statusV["facing"] = Vector2.Down;
            dir = Vector2.Zero;
        }
        
        float acc = moveAcceleration;
        if (dir.X * self.Velocity.X < -10f)
            acc *= 5;
        if (dir.Y * self.Velocity.Y < -10f)
            acc *= 5;
        
        if(statusF["dash"] > 0.5f)
        {
            dir *= velocityRefValue2;
            acc = 50;
        }
        else
            dir *= velocityRefValue1;
        
        self.Velocity = self.Velocity.MoveToward(dir, (float)(delta * acc));

        Vector2 v2 = self.Velocity;
        self.Velocity += statusV["inertia"];
        self.MoveAndSlide();
        self.Velocity = v2;

        KinematicCollision2D coll = self.GetLastSlideCollision();
        if (coll != null)
        {
            Vector2 n = coll.GetNormal();
            if (Math.Abs(n.Dot(new Vector2(1, 0))) > .75f || Math.Abs(n.Dot(new Vector2(0, 1))) > .75f)
                statusF["wall"] = 1;
            else
                statusF["wall"] = 0;
        }
        else
        {
            statusF["wall"] = 0;
        }
    }
}
