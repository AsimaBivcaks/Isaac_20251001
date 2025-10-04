using Godot;
using System;

public partial class ItemObj : TriggerObj
{
    //private const float MAX_VELOCITY = 80.0f;
    //private const float FRICTION = 0.85f;
    private const float JELLY_SQUEEZE_STRENGTH = 7f;

    [Export] public Item item;

    private ItemObjJellyEffect JellyEffect = null;

    private Sprite2D sprite;
    private RigidBody2D collTest;
    private Texture2D itemTexture;

    //private Vector2 Velocity = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();

        sprite = GetNode<Sprite2D>("Spr");
        collTest = GetNode<RigidBody2D>("CollTest");

        sprite.Texture = item.icon;
    }
    
    public override void _Process(double delta)
    {
        /*if (Velocity.Length() > 0.1f)
        {
            KinematicCollision2D coll = collTest.MoveAndCollide(Velocity * (float)delta, true);
            if (coll != null)
            {
                Velocity = Velocity.Bounce(coll.GetNormal()) * 0.5f;
            }
            Position += Velocity * (float)delta;
            Velocity = Velocity.MoveToward(Vector2.Zero, FRICTION * (float)delta);
            if (Velocity.Length() > MAX_VELOCITY) Velocity = Velocity.Normalized() * MAX_VELOCITY;
        }
        else
        {
            Velocity = Vector2.Zero;
        }*/

        Position += collTest.Position;
        collTest.Position = Vector2.Zero;

        JellyEffect?._Process((float)delta);
    }

    protected void GetJellyEffect()
    {
        JellyEffect = new ItemObjJellyEffect(sprite);
    }

    public void Push(Vector2 dir, float force)
    {
        //Velocity += dir.Normalized() * force;
        collTest.ApplyCentralImpulse(dir.Normalized() * force);
        //if (Velocity.Length() > MAX_VELOCITY) Velocity = Velocity.Normalized() * MAX_VELOCITY;
        dir = dir.Abs();
        if ( dir.X > dir.Y )
        {
            JellyEffect?.Squeeze(JELLY_SQUEEZE_STRENGTH);
        }
        else
        {
            JellyEffect?.Squeeze(-JELLY_SQUEEZE_STRENGTH);
        }
    }

    public bool Get(Player player)
    {
        if (item.IsPickable(player))
        {
            item.OnPlayerGet(player);
            return true;
        }
        return false;
    }

    public void Destroy()
    {
        QueueFree();
    }

    private static PackedScene scene = GD.Load<PackedScene>(WorldUtilsPools.resourcePaths["item_obj"]);
    public static ItemObj Create(Node mount, Vector2 position, Item item, bool withJelly = true)
    {
        ItemObj obj = (ItemObj)scene.Instantiate();
        obj.item = item;
        obj.InitAndEnterTree(mount, position);
        if (withJelly)
        {
            obj.GetJellyEffect();
        }
        return obj;
    }
}