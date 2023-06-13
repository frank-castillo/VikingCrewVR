using System;
using UnityEngine;

public enum BeatTierType
{
    None = -1,
    T1,
    T2,
    T3
}

public enum DrumSide
{
    None = -1,
    Left,
    Right,
}

public enum HammerSide
{
    None = -1,
    Left,
    Right,
}

public enum HapticIntensity
{
    None = -1,
    Low,
    High,
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

public enum SFXType
{
    None = -1,
    OffBeatDrum,
    OnBeatDrum,
    VikingChant,
    PaddleRow,
    SplashLeft,
    SplashRight,
    DrumHum,
    DrumVacuum,
    Torch,
    ShieldSmash,
    RuneGlow
}

public enum VikingAnimationType
{
    None = -1,
    Idle,
    Row,
    Stretch,
    Yawn,
}

public class Enums
{
    public static void InvalidSwitch(Type script, Type type)
    {
        Debug.LogError($"Enum type [{type}] in the {script} is invalid, please fix!");
    }
}