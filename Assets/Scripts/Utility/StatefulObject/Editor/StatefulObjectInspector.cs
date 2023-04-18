using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
[CanEditMultipleObjects]
[CustomEditor(typeof(StatefulObject), true)]
public class StatefulObjectInspector : Editor
{
    private GameObject _transformTarget = null;
    private ReorderableList _reorderableList;
    private SerializedProperty _setDefaultStateOnBadStateRequestProp;
    
    private void OnEnable()
    {
        StatefulObject statefulObject = target as StatefulObject;
        if (statefulObject == null) return;
        
        _setDefaultStateOnBadStateRequestProp = serializedObject.FindProperty("SetDefaultStateOnBadStateRequest");
        _transformTarget = statefulObject.gameObject;
        if (statefulObject.StateEntries.Count == 0 && _transformTarget != null)
        {
            statefulObject.SetupValuesFromTarget(_transformTarget.transform);
        }

        _reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("stateEntries"), true, true, true, true)
        {
            drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "State Entries"); },
            drawElementCallback = DrawListElement
        };
    }
    
    private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        StatefulObject statefulObject = target as StatefulObject;
        if (statefulObject == null) return;
        
        StatefulObject.StateEntry item;
        if (index >= statefulObject.StateEntries.Count)
        {
            return;
        }
        else
        {
            item = statefulObject.StateEntries[index];
        }

        Rect stateNameArea = new Rect(rect)
        {
            width = rect.width * 0.333f
        };
        
        item.StateName = EditorGUI.TextField(stateNameArea, item.StateName);
        int padding = 4;
        Rect objectReferenceArea = new Rect(rect)
        {
            x = stateNameArea.x + stateNameArea.width + padding, width = rect.width - stateNameArea.width - padding
        };
        
        item.StateObject = EditorGUI.ObjectField(objectReferenceArea, item.StateObject, typeof(GameObject), true) as GameObject;
    }
    
    public override void OnInspectorGUI()
    {
        StatefulObject statefulObject = target as StatefulObject;
        if (statefulObject == null) return;
        
        serializedObject.Update();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Setup From Transform Group"))
        {
            if (_transformTarget == null)
            {
                Debug.LogWarning("There was no transform group set");
            }
            else
            {
                statefulObject.SetupValuesFromTarget(_transformTarget.transform);
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
                Object selected = Selection.activeObject;
                Selection.activeObject = null;
                Selection.activeObject = selected;
                return;
            }
        }
        
        //using (new ColourLayout(Color.grey))
        {
            _transformTarget = EditorGUILayout.ObjectField(_transformTarget, typeof(GameObject), true) as GameObject;
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_setDefaultStateOnBadStateRequestProp);
        if (statefulObject.CurrentState == null || statefulObject.CurrentState.IsValid == false)
        {
            statefulObject.SetToDefaultState();
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultStateName"));
        if (statefulObject.StateEntries.Count > 1)
        {
            EditorGUILayout.LabelField("Cycle states");
            using (new EditorGUILayout.HorizontalScope())
            {
                int currentStateIndex = statefulObject.GetCurrentStateIndex();
                if (currentStateIndex != -1)
                {
                    if (GUILayout.Button("<", EditorStyles.miniButtonLeft))
                    {
                        --currentStateIndex;
                        if (currentStateIndex < 0)
                        {
                            currentStateIndex = statefulObject.StateEntries.Count - 1;
                        }

                        statefulObject.SetState(statefulObject.GetStateFromIndex(currentStateIndex).StateName);
                    }
                    if (GUILayout.Button(">", EditorStyles.miniButtonRight))
                    {
                        currentStateIndex = (currentStateIndex + 1) % statefulObject.StateEntries.Count;
                        statefulObject.SetState(statefulObject.GetStateFromIndex(currentStateIndex).StateName);
                    }
                }
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        _reorderableList.DoLayoutList();
        for (int i = 0; i < statefulObject.StateEntries.Count; ++i)
        {
            if (statefulObject.StateEntries[i] != null && !string.IsNullOrEmpty(statefulObject.StateEntries[i].StateName))
            {
                if (statefulObject.StateEntries[i].StateObject != null && statefulObject.CurrentState.StateObject != null)
                {
                    if (statefulObject.StateEntries[i].StateObject.GetInstanceID() == statefulObject.CurrentState.StateObject.GetInstanceID())
                    {
                        // Highlight the current state as green
                        GUI.backgroundColor = Color.green;
                    }
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(statefulObject.StateEntries[i].StateName, EditorStyles.miniButtonLeft))
                    {
                        statefulObject.SetState(statefulObject.StateEntries[i].StateName, true);
                    }
                    if (GUILayout.Button(">", EditorStyles.miniButtonRight, GUILayout.Width(20)))
                    {
                        Selection.objects = new UnityEngine.Object[] { statefulObject.StateEntries[i].StateObject };
                    }
                }
                GUI.backgroundColor = GUI.color;
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
