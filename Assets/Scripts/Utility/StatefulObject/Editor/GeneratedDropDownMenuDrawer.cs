using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Custom property drawer to display a drop down menu generated from an array of strings
/// </summary>
[CustomPropertyDrawer(typeof(GeneratedDropDownMenuAttribute))]
public class GeneratedDropDownMenuDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GeneratedDropDownMenuAttribute dropDownAttribute = attribute as GeneratedDropDownMenuAttribute;
        Object target = property.serializedObject.targetObject;

        Type type = dropDownAttribute.Type == null ? target.GetType() : dropDownAttribute.Type;
        MethodInfo methodInfo = type.GetMethod(dropDownAttribute.FunctionName,
            BindingFlags.CreateInstance | BindingFlags.Static |
            BindingFlags.Public | BindingFlags.NonPublic
            //BindingFlags.FlattenHierarchy
        );

        if (methodInfo == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        string[] options = methodInfo.Invoke(target, null) as string[];
        if (options == null || options.Length == 0)
        {
            if (dropDownAttribute.UseDefaultBehaviorIfEmpty)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            options = new string[] { }; // Empty string array
        }

        EditorGUI.BeginProperty(position, label, property);
        int currentIndex = 0;
        for (int i = options.Length - 1; i >= 0; --i)
        {
            if (options[i].Equals(property.stringValue, StringComparison.Ordinal))
            {
                currentIndex = i;
                break;
            }
        }

        Rect prefixLabelRect = new Rect(position);
        Rect popupRect = new Rect(position);

        prefixLabelRect.width = position.width * 0.5f;
        popupRect.x = prefixLabelRect.x + prefixLabelRect.width;
        popupRect.width = position.width * 0.5f;

        EditorGUI.LabelField(prefixLabelRect, property.displayName);
        currentIndex = EditorGUI.Popup(popupRect, currentIndex, options);
        if (currentIndex >= 0 && currentIndex < options.Length)
        {
            property.stringValue = options[currentIndex];
            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
}