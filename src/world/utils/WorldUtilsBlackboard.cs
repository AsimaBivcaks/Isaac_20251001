using Godot;
using System;
using System.Collections.Generic;

public static class WorldUtilsBlackboard
{
    private static Dictionary<string, object> data = new Dictionary<string, object>();

    static WorldUtilsBlackboard()
    {
        Set("decision_frequency", .2f);
        Set("current_bosses", new List<Character>());
        Set("player_instance", null);
        Set("screen_size", new Vector2I(480, 270));
    }

    public static void Set(string key, object value)
    {
        data[key] = value;
    }

    public static T Get<T>(string key)
    {
        if (data.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }
        return default(T);
    }
}