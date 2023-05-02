using System;
using UnityEngine;

public enum OnHitBeatType
{
    None = -1,
    T1,
    T2,
    T3
}

public class Enums
{
    public static void InvalidSwitch(Type script, Type type)
    {
        Debug.LogError($"Enum type [{type}] in the {script} is invalid, please fix!");
    }
}