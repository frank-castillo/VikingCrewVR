using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (GameEvent)target;

        if (GUILayout.Button("Preview", GUILayout.Height(30)))
        {
            script.Invoke();
        }
    }
}