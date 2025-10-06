using Godot;
using System;
using System.Collections.Generic;

using WURM = WorldUtilsRoomManager;

public class WorldGenerator
{
    public SpawnPool roomPool;

    //Randomized B/DFS
    public void GenerateWorldFrom(Vector2I startRoom)
    {
        List<Vector2I> borders = new List<Vector2I>(){ startRoom };
        while(borders.Count > 0)
        {
            int index = WorldUtilsRandom.RandomiRange(0, borders.Count - 1);
            Vector2I current = borders[index];
            borders.RemoveAt(index);
            if (WURM.CheckRoomAt(current)) continue;
            string room = roomPool.RollRaw();
            bool failedroll = false;
            while(!WURM.TryArrangeRoomAt(room, current.X, current.Y))
            {
                if (WorldUtilsRandom.Chance(0.1f))
                {
                    failedroll = true;
                    break;
                }
                room = roomPool.RollRaw();
            }
            if (failedroll) continue;
            RoomSpace rs = WorldUtilsPools.GetRoomSpace(room);
            var accessibles = RoomSpace.Accessibles[RoomSpace.GetCode(rs)];
            if (accessibles == null) continue;
            foreach(var dir in accessibles)
            {
                Vector2I next = current + dir;
                if ( WURM.CheckRoomAt(next) ) continue;
                if ( !borders.Contains(next) )
                    borders.Add(next);
            }
            if (WorldUtilsRandom.Chance(0.2f))
                break;
        }
    }
}