using UnityEngine;
using System.Collections;

/// <summary>
/// Attribute that utilizes the GeneratedDropDownMenuPropertyDrawer to draw a drop down menu on the inspector
/// </summary>
public class GeneratedDropDownMenuAttribute : PropertyAttribute
{
    public string FunctionName { get; private set; }
    public bool UseDefaultBehaviorIfEmpty { get; private set; }
    public System.Type Type { get; private set; }

    public GeneratedDropDownMenuAttribute(string functionName, bool useDefaultBehaviorIfEmpty = false)
    {
        this.FunctionName = functionName;
        this.UseDefaultBehaviorIfEmpty = useDefaultBehaviorIfEmpty;
    }

    public GeneratedDropDownMenuAttribute(string functionName, System.Type type, bool useDefaultBehaviorIfEmpty = false)
    {
        this.FunctionName = functionName;
        this.Type = type;
        this.UseDefaultBehaviorIfEmpty = useDefaultBehaviorIfEmpty;
    }
}