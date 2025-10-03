using Godot;
using System;

public class KnightMoveBehavior : CharacterBehavior
{
    private static readonly Vector2[] directions = new Vector2[4]{
        Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right,
    };

    private float decisionFrequency = WorldUtilsBlackboard.Get<float>("decision_frequency") * 1.5f;

    private float velocityRefValue1;
    private float velocityRefValue2;
    private float moveAcceleration;
    private float dashDuration = 1.3f;
    private float dashCooldown = 1f;

    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    private float decisionTimer = 0f;

    CharacterBehavior moveOp;
    Player player;

    public KnightMoveBehavior(Character _self, float _velocityRefValue1, float _velocityRefValue2, float _moveAcceleration) : base(_self)
    {
        velocityRefValue1 = _velocityRefValue1;
        velocityRefValue2 = _velocityRefValue2;
        moveAcceleration = _moveAcceleration;
    }
    //i hate this so much

    public override void _Ready()
    {
        base._Ready();

        player = WorldUtilsBlackboard.Get<Player>("player_instance");

        statusV["input"] = Vector2.Zero;
        statusV["facing"] = Vector2.Down;
        statusF["dash"] = 0;
        statusF["wall"] = 0; //0:clear, 1:hit wall
        //kinda inefficient, but whatever

        moveOp = new KnightMoveOperationBehavior(self, velocityRefValue1, velocityRefValue2, moveAcceleration);

        decisionTimer = -1;
    }

    public override void _Process(double delta) //man im too tired to implement a proper state machine
    {
        //AHHH IT'S FXCKING 2AM!
        base._Process(delta);

        if(statusF["wall"] > 0.5f) //stop dashing & turn immediately when hitting wall
        {
            dashTimer = 0;
            if(statusF["dash"] > 0.5f)
            {
                statusF["dash"] = 0;
                dashCooldownTimer = dashCooldown;
            }
            decisionTimer = decisionFrequency;
            do {
                statusV["input"] = directions[WorldUtilsRandom.RandomiRange(0,4)];
            } while (
                self.MoveAndCollide(statusV["input"] * .1f, true) != null //.1f: virtual walk time
                || WorldUtilsRandom.Chance(.1f) //make sure the program dont get stuck when a knight is surrounded by walls
            );
        }

        if (dashTimer > 0)
        {
            dashTimer -= (float)delta;
            Vector2 toPlayer = (player.GlobalPosition - self.GlobalPosition).Normalized();
            statusV["input"] = toPlayer;
            statusF["dash"] = 1;
        }
        else
        {
            if(statusF["dash"] > 0.5f)
                dashCooldownTimer = dashCooldown;
            statusF["dash"] = 0;
        }

        if (decisionTimer > 0) decisionTimer -= (float)delta;
        else
        {
            decisionTimer = decisionFrequency;

            if(statusF["dash"] < 0.5f) //when wondering, dash or keep randomly walking
            {
                Object coll = ((Knight)self).playerDetector.GetCollider();
                if (dashCooldownTimer < .01f && coll != null && coll is Player)
                {
                    dashTimer = dashDuration;
                    self.Velocity = statusV["input"] * velocityRefValue2;
                } else if(WorldUtilsRandom.Chance(.6f)){
                    do {
                        statusV["input"] = directions[WorldUtilsRandom.RandomiRange(0,4)];
                    } while (
                        self.MoveAndCollide(statusV["input"] * .1f, true) != null
                        || WorldUtilsRandom.Chance(.1f)
                    );
                }
            }
        }

        if(dashCooldownTimer > 0)
            dashCooldownTimer -= (float)delta;

        moveOp._Process(delta);
    }
}