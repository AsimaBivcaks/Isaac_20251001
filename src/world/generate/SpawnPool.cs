using Godot;
using System;

[GlobalClass]
public partial class SpawnPool : Resource
{
    [Export] public string PoolName = "default";
    [Export] public string[] Items;
    [Export] public float[] Weights;

    private float totalWeight = -1f;
    private bool equalChanceMode = false;

    private void Initialize()
    {
        if (totalWeight < 0f && Weights != null && Weights.Length == Items.Length)
        {
            totalWeight = 0f;
            foreach (float w in Weights)
                totalWeight += w;
            return;
        }
        equalChanceMode = true;
    }

    public T Roll<T>() where T : class
    {
        if (Items.Length == 0)
            return null;

        if (totalWeight < 0f)
            Initialize();
        
        if (totalWeight < .01f)
            equalChanceMode = true;

        float roll = WorldUtilsRng.Randomf();
        return RollItem<T>(roll);
    }

    private T RollItem<T>(float rollval) where T : class
    {
        if (equalChanceMode)
        {
            int index = (int)(rollval * Items.Length);
            return GetItemByIndex<T>(index);
        }
        else
        {
            float r = rollval * totalWeight;
            for (int i = 0; i < Weights.Length; i++)
            {
                r -= Weights[i];
                if (r <= 0f)
                    return GetItemByIndex<T>(i);
            }
        }
        return null;
    }

    private T GetItemByIndex<T>(int index) where T : class
    {
        if (index < 0 || index >= Items.Length)
            return null;
        string s = Items[index];
        if (string.IsNullOrEmpty(s))
            return null;
        return WorldUtilsPools.GetResource<T>(s);
    }

    public string RollRaw()
    {
        if (Items.Length == 0)
            return null;

        if (totalWeight < 0f)
            Initialize();
        
        if (totalWeight < .01f)
            equalChanceMode = true;

        float roll = WorldUtilsRng.Randomf();
        return RollRawItem(roll);
    }

    private string RollRawItem(float rollval)
    {
        if (equalChanceMode)
        {
            int index = (int)(rollval * Items.Length);
            return Items[index];
        }
        else
        {
            float r = rollval * totalWeight;
            for (int i = 0; i < Weights.Length; i++)
            {
                r -= Weights[i];
                if (r <= 0f)
                    return Items[i];
            }
        }
        return null;
    }
}