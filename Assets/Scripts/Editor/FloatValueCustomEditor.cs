using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatValue))]
public class FloatValueCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (FloatValue)target;

        if (GUILayout.Button("ChangeValueEvent", GUILayout.Height(30)))
        {
            script.Changed.Invoke();
        }
    }
}