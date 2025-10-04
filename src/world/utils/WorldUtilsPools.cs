
using System.Collections.Generic;

public static class WorldUtilsPools
{
    public readonly static Dictionary<string, string> resourcePaths = new Dictionary<string, string>()
    {
        {"proj_tear", "res://scenes/projectiles/Tear.tscn"},
        {"proj_e_bloodtear", "res://scenes/projectiles/e_bloodtear.tscn"},
        {"proj_e_aired_bloodtear", "res://scenes/projectiles/e_aired_bloodtear.tscn"}
    };

    static WorldUtilsPools()
    {

    }
}