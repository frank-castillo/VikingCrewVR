using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform), true)]
public class TransformCustomEditor : Editor
{
    private enum PasteOptions
    {
        Position,
        Rotation,
        Scale
    }

    private bool _showRandoms = false;

    private Vector3 _randPosition = Vector3.zero;
    private Vector3 _randRotation = Vector3.zero;
    private Vector3 _randScale = Vector3.zero;

    private Vector3 _vectorValue1 = Vector3.zero;
    private Vector3 _vectorValue2 = Vector3.zero;
    private Vector3 _vectorValue3 = Vector3.zero;

    public override void OnInspectorGUI()
    {
        var transform = target as Transform;
        if (!transform)
        {
            return;
        }

        EditorGUIUtility.labelWidth = 15.0f;
        var option = GUILayout.MinWidth(30.0f);

        Vector3 position;
        Vector3 rotation;
        Vector3 scale;
        bool isResetValid;
        float buttonWidth = 30.0f;

        // Position
        EditorGUILayout.BeginHorizontal();
        {
            isResetValid = IsResetVectorValid(transform.localPosition, Vector3.zero);
            if (DrawButton("P", "Reset Position", isResetValid, buttonWidth))
            {
                Undo.RecordObject(transform, "Reset Position");
                transform.localPosition = Vector3.zero;
            }
            EditorGUILayout.LabelField("Position", option);
            position = DrawVector3(transform.localPosition);
        }
        EditorGUILayout.EndHorizontal();

        // Rotation
        EditorGUILayout.BeginHorizontal();
        {
            isResetValid = IsResetVectorValid(transform.localEulerAngles, Vector3.zero);
            if (DrawButton("R", "Reset Rotation", isResetValid, buttonWidth))
            {
                Undo.RecordObject(transform, "Reset Rotation");
                transform.localEulerAngles = Vector3.zero;
            }
            EditorGUILayout.LabelField("Rotation", option);
            rotation = DrawVector3(transform.localEulerAngles);
        }
        EditorGUILayout.EndHorizontal();

        // Scale
        EditorGUILayout.BeginHorizontal();
        {
            isResetValid = IsResetVectorValid(transform.localScale, Vector3.one);
            if (DrawButton("S", "Reset Scale", isResetValid, buttonWidth))
            {
                Undo.RecordObject(transform, "Reset Scale");
                transform.localScale = Vector3.one;
            }
            EditorGUILayout.LabelField("Scale", option);
            scale = DrawVector3(transform.localScale);
        }
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            Undo.RecordObject(transform, "Transform Change");
            transform.localPosition = ValidateVector(position);
            transform.localEulerAngles = ValidateVector(rotation);
            transform.localScale = ValidateVector(scale);
        }

        if (GUILayout.Button(_showRandoms ? "Hide" : "Show", GUILayout.Height(15)))
        {
            _showRandoms = !_showRandoms;
        }

        // Check if the right mouse button was clicked
        if (Event.current.type == EventType.ContextClick)
        {
            _vectorValue1 = EditorGUILayout.Vector3Field("Vector Value 1", transform.localPosition);
            _vectorValue2 = EditorGUILayout.Vector3Field("Vector Value 2", transform.localRotation.eulerAngles);
            _vectorValue3 = EditorGUILayout.Vector3Field("Vector Value 3", transform.localScale);

            // Create a new GenericMenu
            GenericMenu menu = new GenericMenu();

            // Add menu items to copy each vector value to clipboard
            menu.AddItem(new GUIContent("Copy Position"), false, CopyVectorValue, _vectorValue1);
            menu.AddItem(new GUIContent("Copy Rotation"), false, CopyVectorValue, _vectorValue2);
            menu.AddItem(new GUIContent("Copy Scale"), false, CopyVectorValue, _vectorValue3);

            // Add a separator to the menu
            menu.AddSeparator("");

            // Add a "Paste" option to the menu
            menu.AddItem(new GUIContent("Paste Position"), false, PasteVectorValue, PasteOptions.Position);
            menu.AddItem(new GUIContent("Paste Rotation"), false, PasteVectorValue, PasteOptions.Rotation);
            menu.AddItem(new GUIContent("Paste Scale"), false, PasteVectorValue, PasteOptions.Scale);

            // Show the context menu
            menu.ShowAsContext();
        }

        if (!_showRandoms)
        {
            return;
        }

        // Randomize Position
        EditorGUILayout.BeginHorizontal();
        {
            Transform[] selectedTransforms = Selection.transforms;
            isResetValid = IsResetVectorValid(_randPosition, Vector3.zero);
            if (DrawButton("RP", "Randomize Position", isResetValid, buttonWidth))
            {
                RandomPosition(_randPosition, selectedTransforms);
            }
            EditorGUILayout.LabelField("Rand Pos", EditorStyles.boldLabel, option);
            _randPosition = DrawVector3(_randPosition);
        }
        EditorGUILayout.EndHorizontal();

        // Randomize Rotation
        EditorGUILayout.BeginHorizontal();
        {
            Transform[] selectedTransforms = Selection.transforms;
            isResetValid = IsResetVectorValid(_randRotation, Vector3.zero);
            if (DrawButton("RR", "Randomize Rotation", isResetValid, buttonWidth))
            {
                RandomRotation(_randRotation, selectedTransforms);
            }
            EditorGUILayout.LabelField("Rand Rot", EditorStyles.boldLabel, option);
            _randRotation = DrawVector3(_randRotation);
        }
        EditorGUILayout.EndHorizontal();

        // Randomize Scale
        EditorGUILayout.BeginHorizontal();
        {
            Transform[] selectedTransforms = Selection.transforms;
            isResetValid = IsResetVectorValid(_randScale, Vector3.zero);
            if (DrawButton("RS", "Randomize Scale", isResetValid, buttonWidth))
            {
                RandomScale(_randScale, selectedTransforms);
            }
            EditorGUILayout.LabelField("Rand Scale", EditorStyles.boldLabel, option);
            _randScale = DrawVector3(_randScale);
        }
        EditorGUILayout.EndHorizontal();
    }

    private static bool DrawButton(string title, string tooltip, bool enabled, float width)
    {
        if (enabled)
        {
            return GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width));
        }

        var color = GUI.color;
        GUI.color = new Color(1f, 1f, 1f, 0.25f);
        GUILayout.Button(new GUIContent(title, tooltip), GUILayout.Width(width));
        GUI.color = color;
        return false;
    }

    private static Vector3 DrawVector3(Vector3 value)
    {
        var option = GUILayout.MinWidth(30f);
        value.x = EditorGUILayout.FloatField("X", value.x, option);
        value.y = EditorGUILayout.FloatField("Y", value.y, option);
        value.z = EditorGUILayout.FloatField("Z", value.z, option);
        return value;
    }

    private static bool IsResetVectorValid(Vector3 vector, Vector3 target)
    {
        return (vector.x != target.x || vector.y != target.y || vector.z != target.z);
    }

    private static Vector3 ValidateVector(Vector3 vector)
    {
        vector.x = float.IsNaN(vector.x) ? 0.0f : vector.x;
        vector.y = float.IsNaN(vector.y) ? 0.0f : vector.y;
        vector.z = float.IsNaN(vector.z) ? 0.0f : vector.z;
        return vector;
    }

    private void RandomPosition(Vector3 randAmt, Transform[] t)
    {
        for (int i = 0; i < t.Length; ++i)
        {
            Vector3 temp = t[i].position;

            temp.x = Random.Range(temp.x - randAmt.x, temp.x + randAmt.x);
            temp.y = Random.Range(temp.y - randAmt.y, temp.y + randAmt.y);
            temp.z = Random.Range(temp.z - randAmt.z, temp.z + randAmt.z);

            Undo.RecordObject(t[i], "Random position " + t[i].name);
            t[i].position = temp;
        }
    }

    private void RandomRotation(Vector3 randAmt, Transform[] t)
    {
        for (int i = 0; i < t.Length; ++i)
        {
            Vector3 temp = t[i].localEulerAngles;

            temp.x = Random.Range(temp.x - randAmt.x, temp.x + randAmt.x);
            temp.y = Random.Range(temp.y - randAmt.y, temp.y + randAmt.y);
            temp.z = Random.Range(temp.z - randAmt.z, temp.z + randAmt.z);

            Undo.RecordObject(t[i], "Random Rotation " + t[i].name);
            t[i].localEulerAngles = temp;
        }
    }

    private void RandomScale(Vector3 randAmt, Transform[] t)
    {
        for (int i = 0; i < t.Length; ++i)
        {
            Vector3 temp = t[i].localScale;

            temp.x = Random.Range(temp.x - randAmt.x, temp.x + randAmt.x);
            temp.y = Random.Range(temp.y - randAmt.y, temp.y + randAmt.y);
            temp.z = Random.Range(temp.z - randAmt.z, temp.z + randAmt.z);

            Undo.RecordObject(t[i], "Random Scale " + t[i].name);
            t[i].localScale = temp;
        }
    }

    private void CopyVectorValue(object value)
    {
        Vector3 vectorValue = (Vector3)value;

        // Copy the vector value to clipboard
        EditorGUIUtility.systemCopyBuffer = vectorValue.ToString();
    }

    private void PasteVectorValue(object value)
    {
        string clipboardString = EditorGUIUtility.systemCopyBuffer;
        PasteOptions options = (PasteOptions)value;
        Vector3 pastedVectorValue = Vector3.zero;

        if (TryParse(clipboardString, out pastedVectorValue))
        {
            var transform = target as Transform;
            switch (options)
            {
                case PasteOptions.Position:
                    Undo.RecordObject(transform, "Reset Position");
                    transform.localPosition = pastedVectorValue;
                    break;
                case PasteOptions.Rotation:
                    Undo.RecordObject(transform, "Reset Rotation");
                    transform.localEulerAngles = pastedVectorValue;
                    break;
                case PasteOptions.Scale:
                    Undo.RecordObject(transform, "Reset Scale");
                    transform.localScale = pastedVectorValue;
                    break;
            }
        }
    }

    private bool TryParse(string s, out Vector3 result)
    {
        result = Vector3.zero;
        s = s.Replace("(", "").Replace(")", "");
        string[] values = s.Split(',');

        if (values.Length == 3)
        {
            float x, y, z;
            if (float.TryParse(values[0], out x) && float.TryParse(values[1], out y) && float.TryParse(values[2], out z))
            {
                result = new Vector3(x, y, z);
                return true;
            }
        }
        return false;
    }
}
