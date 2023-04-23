using System;
using UnityEngine;

public enum SceneType
{
    None = -1,
    MainMenu
}

public class Enums
{
    public static void InvalidSwitch(Type script, Type type)
    {
        Debug.LogError($"Enum type [{type}] in the {script} is invalid, please fix!");
    }
}