using System;
using UnityEngine;

public enum OnHitBeatType
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

public class Enums
{
    public static void InvalidSwitch(Type script, Type type)
    {
        Debug.LogError($"Enum type [{type}] in the {script} is invalid, please fix!");
    }
}