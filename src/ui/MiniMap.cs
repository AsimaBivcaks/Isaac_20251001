using Godot;
using System;

using WURM = WorldUtilsRoomManager;

public partial class MiniMap : Panel
{
    private static readonly Color COLOR_ROOM_UNKNOWN = new Color(0f, 0f, 0f, 0f);
    private static readonly Color COLOR_ROOM_UNVISITED = new Color(0.2f, 0.2f, 0.2f);
    private static readonly Color COLOR_ROOM_VISITED = new Color(0.5f, 0.5f, 0.5f);
    private static readonly Color COLOR_ROOM_CURRENT = new Color(1f, 1f, 1f);
    private const int ROOM_SIZE_X = 8;
    private const int ROOM_SIZE_Y = 7;
    private const int HFRAMES = 4;
    private const int VFRAMES = 2;
    private const float SCALE = 3f;
    private const int ICO_HFRAMES = 8;
    private const int ICO_VFRAMES = 11;
    private static readonly int[] ROOMCODE2FRAME =
    [
        -1, 3, -1, 2, -1, 1, -1, 7, -1, -1, -1, 6, -1, 5, 4, 0,
    ];

    [Export] private Texture2D roomsTex;
    [Export] private Texture2D iconsTex;

    private Node2D roomContainer;
    private Sprite2D[,] rooms;
    private Sprite2D[,] icons;
    private bool[,] iconfixed;
    
    public override void _Ready()
    {
        base._Ready();
        WURM.RoomSwitched += OnRoomSwitched;
    }

    public void Init()
    {
        roomContainer = new Node2D();
        AddChild(roomContainer);
        roomContainer.Position = Vector2.Zero;
        roomContainer.Scale = new Vector2(SCALE, SCALE);
        roomContainer.Visible = false;

        rooms = new Sprite2D[WURM.MAX_ROOMS_X, WURM.MAX_ROOMS_Y];
        for ( int i = 0; i < WURM.MAX_ROOMS_X; i++ )
            for ( int j = 0; j < WURM.MAX_ROOMS_Y; j++ )
                if(WURM.CheckRoomAt(new Vector2I(i, j)) && WURM.RoomArrangementOffsets[i, j] == Vector2I.Zero)
                {
                    var rs = WorldUtilsPools.GetRoomSpace(WURM.RoomArrangement[i, j]);
                    uint code = RoomSpace.GetCode(rs);
                    Sprite2D spr = new Sprite2D();
                    spr.Texture = roomsTex;
                    spr.Centered = false;
                    spr.Hframes = HFRAMES;
                    spr.Vframes = VFRAMES;
                    spr.Position = new Vector2(i * ROOM_SIZE_X, j * ROOM_SIZE_Y);
                    spr.Modulate = COLOR_ROOM_UNKNOWN;
                    int frame = ROOMCODE2FRAME[code];
                    if (frame < 0 || frame >= HFRAMES * VFRAMES)
                    {
                        GD.PrintErr("Invalid room code ", code, " for room ", WURM.RoomArrangement[i, j]);
                        frame = 0;
                    }
                    spr.Frame = frame;
                    roomContainer.AddChild(spr);
                    rooms[i, j] = spr;
                    if (rs.Is14) //Their sprites stay in the top-right corner
                        spr.Position -= new Vector2(ROOM_SIZE_X, 0);
                }
        
        icons = new Sprite2D[WURM.MAX_ROOMS_X, WURM.MAX_ROOMS_Y];
        iconfixed = new bool[WURM.MAX_ROOMS_X, WURM.MAX_ROOMS_Y];
        for ( int i = 0; i < WURM.MAX_ROOMS_X; i++ )
            for ( int j = 0; j < WURM.MAX_ROOMS_Y; j++ )
                if(rooms[i,j] != null)
                {
                    int icon = WorldUtilsPools.FixedRoomIcon(WURM.RoomArrangement[i, j]);
                    Sprite2D spr = new Sprite2D();
                    spr.Texture = iconsTex;
                    spr.Centered = false;
                    spr.Hframes = ICO_HFRAMES;
                    spr.Vframes = ICO_VFRAMES;
                    spr.Position = new Vector2(i * ROOM_SIZE_X, j * ROOM_SIZE_Y) + new Vector2(1,1);
                    spr.Modulate = COLOR_ROOM_UNKNOWN;
                    spr.Frame = icon;
                    spr.Scale = new Vector2(0.5f, 0.5f);
                    roomContainer.AddChild(spr);
                    if (icon < 81)
                        iconfixed[i, j] = true;
                    icons[i, j] = spr;
                }
    }

    private void OnRoomSwitched(Room r1, Room r2)
    {
        if (r1 == null)
        {
            StatusInit();
        }
        else
        {
            Vector2I p1 = r1.GridPosition;
            if (rooms[p1.X, p1.Y] != null)
                rooms[p1.X, p1.Y].Modulate = COLOR_ROOM_VISITED;
            UpdateIcon(r1);
        }

        Vector2I pos = r2.GridPosition;
        roomContainer.Position =
            new Vector2(-pos.X * ROOM_SIZE_X, -pos.Y * ROOM_SIZE_Y) * SCALE +
            new Vector2(48, 48) -
            new Vector2(ROOM_SIZE_X, ROOM_SIZE_Y) * SCALE / 2f;

        if (rooms[pos.X, pos.Y] != null)
            rooms[pos.X, pos.Y].Modulate = COLOR_ROOM_CURRENT;
        if (icons[pos.X, pos.Y] != null)
            icons[pos.X, pos.Y].Modulate = COLOR_ROOM_CURRENT;
        
        UpdateIcon(r2);

        foreach ( var child in r2.GetChildren() )
        {
            if( child is Door door )
            {
                Vector2I apos = pos + door.LocalGrid + door.LeadsTo;
                if ( apos.X < 0 || apos.X >= WURM.MAX_ROOMS_X ||
                    apos.Y < 0 || apos.Y >= WURM.MAX_ROOMS_Y )
                    continue;
                if (WURM.CheckRoomAt(apos))
                    apos -= WURM.RoomArrangementOffsets[apos.X, apos.Y];
                if ( rooms[apos.X, apos.Y] != null &&
                    rooms[apos.X, apos.Y].Modulate == COLOR_ROOM_UNKNOWN )
                {
                    rooms[apos.X, apos.Y].Modulate = COLOR_ROOM_UNVISITED;
                    icons[apos.X, apos.Y].Modulate = COLOR_ROOM_CURRENT;
                }
            }
        }
    }

    private void UpdateIcon(Room rm)
    {
        Vector2I pos = rm.GridPosition;
        if (!iconfixed[pos.X, pos.Y] && icons[pos.X, pos.Y] != null)
        {
            GD.Print("Updating icon for room ", rm.GridPosition);
            icons[pos.X, pos.Y].Frame = 82; //default icon
            if (rm.trackedItems.Count == 0) return;
            foreach ( var itemobj in rm.trackedItems )
            {
                if (itemobj == null || !IsInstanceValid(itemobj) || itemobj is not ItemObj) continue;
                Item it = (itemobj as ItemObj).item;
                if (it is EffectItem or UsableItem)
                {
                    icons[pos.X, pos.Y].Frame = 32;
                    break;
                }
                if (WorldUtilsPools.itemIcons.ContainsKey(it.ItemName))
                {
                    icons[pos.X, pos.Y].Frame = WorldUtilsPools.itemIcons[it.ItemName];
                    break;
                }
            }
        }
    }

    private void StatusInit()
    {
        if (roomContainer == null)
            Init();
        roomContainer.Visible = true;
    }
}
