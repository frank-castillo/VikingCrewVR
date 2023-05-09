using System;
using UnityEngine;

public enum BeatTierType
{
    None = -1,
    T1,
    T2,
    T3
}

public enum SFXType
{
    None = -1,
    OffBeatDrum,
    OnBeatDrum,
    VikingChant,
}

public enum LayerType
{
    None = -1,
    Hammer = 8
}

public enum RowType
{
    None = -1,
    StartRowing,
    StopRowing,
}

public class Enums
{
    public static void InvalidSwitch(Type script, Type type)
    {
        Debug.LogError($"Enum type [{type}] in the {script} is invalid, please fix!");
    }
}