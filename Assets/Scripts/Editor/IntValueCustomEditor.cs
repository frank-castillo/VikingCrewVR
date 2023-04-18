using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IntValue))]
public class IntValueCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (IntValue)target;

        if (GUILayout.Button("ChangeValueEvent", GUILayout.Height(30)))
        {
            script.Changed.Invoke();
        }
    }
}