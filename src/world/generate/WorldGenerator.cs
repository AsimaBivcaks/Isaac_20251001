using Godot;
using System;
using System.Collections.Generic;

using WURM = WorldUtilsRoomManager;

public class WorldGenerator
{
    public const int MIN_ROOMS = 7;

    public float roomLockedChance = 0.07f;
    public SpawnPool roomPool;

    //Randomized B/DFS
    public void GenerateWorldFrom(Vector2I startRoom)
    {
        List<Vector2I> borders = new List<Vector2I>(){ startRoom };
        bool isFirst = true;
        int roomsPlaced = 0;

        while(borders.Count > 0)
        {
            int index = WorldUtilsRng.RandomiRange(0, borders.Count - 1);
            Vector2I current = borders[index];
            borders.RemoveAt(index);
            if (WURM.CheckRoomAt(current)) continue;
            string room;
            if (isFirst)
            {
                room = "room_start";
                isFirst = false;
            }
            else
            {
                room = roomPool.RollRaw();
            }
            bool failedroll = false;
            while(!WURM.TryArrangeRoomAt(room, current.X, current.Y))
            {
                if (WorldUtilsRng.Chance(0.1f))
                {
                    failedroll = true;
                    break;
                }
                room = roomPool.RollRaw();
            }
            if (failedroll)
            {
                if (roomsPlaced < MIN_ROOMS)
                {
                    borders.Add(current);
                }
                continue;
            }
            roomsPlaced++;
            RoomSpace rs = WorldUtilsPools.GetRoomSpace(room);
            var code = RoomSpace.GetCode(rs);
            if (WorldUtilsRng.Chance(roomLockedChance))
                WURM.LockRoomAt(current, code);
            var accessibles = RoomSpace.Accessibles[code];
            if (accessibles == null) continue;
            foreach(var dir in accessibles)
            {
                Vector2I next = current + dir;
                if ( WURM.CheckRoomAt(next) ) continue;
                if ( !borders.Contains(next) )
                    borders.Add(next);
            }
            if (WorldUtilsRng.Chance(0.2f) && roomsPlaced > MIN_ROOMS)
                break;
        }

        bool bossPlaced = false;
        foreach(var possiblePlace in borders)
        {
            if (MiscUtils.AxisDistance(possiblePlace, startRoom) < 3) continue;
            if (WURM.CheckRoomAt(possiblePlace)) continue;
            if (WURM.TryArrangeRoomAt("room_boss", possiblePlace.X, possiblePlace.Y))
            {
                GD.Print("Boss placed at ", possiblePlace);
                WURM.BossRooms.Add(possiblePlace);
                bossPlaced = true;
                break;
            }
        }
        if (!bossPlaced)
        {
            foreach(var possiblePlace in borders)
            {
                if (WURM.CheckRoomAt(possiblePlace)) continue;
                if (WURM.TryArrangeRoomAt("room_boss", possiblePlace.X, possiblePlace.Y))
                {
                    GD.Print("Boss placed at ", possiblePlace);
                    WURM.BossRooms.Add(possiblePlace);
                    bossPlaced = true;
                    break;
                }
            }
        }
    }
}