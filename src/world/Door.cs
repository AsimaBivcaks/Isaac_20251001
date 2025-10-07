using Godot;
using System;

public partial class Door : Node2D
{
    //unlock's the trigger for opening the locked doors
    //enter's the trigger for entering the next room, stays behind the block
    //block's the wall blocking the player from the enter-trigger, stays behind the unlock-trigger
    [Export] private NodePath UnlockPath;
    [Export] private NodePath EnterPath;
    [Export] private NodePath BlockPath;
    [Export] private NodePath AnimatorPath;

    [Export] public Vector2I LocalGrid; //the local grid position of the door; (0,0):LT quater of the room
    [Export] public Vector2I LeadsTo; //the direction the door leads to
    [Export] public bool Locked = false; //if the door needs a key to open
    public Room room;


    public bool OpenAllowed = false;

    private DoorTriggerObj unlock;
    private DoorTriggerObj enter;
    private Node2D block;
    private AnimationPlayer animator;

    public override void _Ready()
    {
        base._Ready();

        //TEMP
        room = WorldUtilsRoomManager.CurrentRoom;

        unlock = GetNode<DoorTriggerObj>(UnlockPath);
        enter = GetNode<DoorTriggerObj>(EnterPath);
        block = GetNode<Node2D>(BlockPath);
        animator = GetNode<AnimationPlayer>(AnimatorPath);

        unlock.OnEnter = Callable.From(OnTryUnlock);
        enter.OnEnter = Callable.From(OnTryEnter);
    }
    
    //Called after ALL other room preparations
    public void FirstEnteredThisRoom(Room room)
    {
        this.room = room;
        if (!WorldUtilsRoomManager.CheckRoomAt( room.GridPosition + LocalGrid + LeadsTo ))
        {
            var filler = WorldUtilsPools.GetResource<PackedScene>("doorfiller")?.Instantiate<DoorFiller>();
            //if (filler == null) return; //let me see the error
            filler.LocalGrid = LocalGrid;
            filler.LeadsTo = LeadsTo;
            room.AddChild(filler);
            QueueFree();
            return;
        }
        
        var target = room.GridPosition + LocalGrid + LeadsTo;
        if (WorldUtilsRoomManager.LockedRooms.Contains(target))
        {
            Locked = true;
            Room rm = WorldUtilsRoomManager.Rooms[target.X, target.Y];
            if (rm != null && rm.RoomCleared)
                Locked = false;
        }

        OpenAllowed = false;
        if (Locked)
        {
            animator.Play("close_lock");
        }
        else
        {
            animator.Play("close");
            unlock.QueueFree();
        }

        Room.DoorPosCalc( this, LocalGrid, LeadsTo );
    }

    //Called by Room
    public void RoomCleared()
    {
        OpenAllowed = true;
        if (!Locked)
        {
            animator.Play("open");
            block.QueueFree();
        }
        else
        {
        }
    }

    public void OnTryEnter()
    {
        if (!OpenAllowed || Locked) return;
        
        World world = World.Instance;
        if (world == null) return;
        world.IntoDoor(LocalGrid, LeadsTo);
    }

    public void OnTryUnlock()
    {
        GD.Print("Trying to unlock door");
        if (!OpenAllowed) return;
        var player = WorldUtilsBlackboard.Get<Player>("player_instance");
        if (player == null) return;
        var key = player.GetBehavior<PlayerKeyManagementBehavior>(BehaviorType.PlayerKeyManagement);
        if (key == null) return;
        GD.Print("Player has " + key.Keys + " keys");
        if (key.UseKey())
        {
            Locked = false;
            animator.Play("open_lock");
            animator.AnimationFinished += OnUnlockAnimFinished;
        }
    }

    public void OnUnlockAnimFinished(StringName str)
    {
        unlock.QueueFree();
        block.QueueFree();
        animator.AnimationFinished -= OnUnlockAnimFinished;
    }
}