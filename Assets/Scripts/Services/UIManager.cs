using Liminal.Core.Fader;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIManager : MonoBehaviour
{
    [SerializeField] private const float _defaultFadeTime = 2.0f;
    public UIManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void FadeToClear(float fadeInTime = _defaultFadeTime)
    {
        ScreenFader.Instance.FadeToClearFromBlack(fadeInTime);
    }

    public void FadeToBlack(float fadeOutTime = _defaultFadeTime)
    {
        ScreenFader.Instance.FadeToBlack(fadeOutTime);
    }

    //TODO: Delete this after tests are done!
    #region Editor

#if UNITY_EDITOR
    [CustomEditor(typeof(UIManager))]
    public class UIManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UIManager customUIManager = (UIManager)target;

            GUILayout.Space(10F);
            GUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Fade Out Screen"))
            {
                customUIManager.FadeToBlack();
            }

            if (GUILayout.Button("Fade In Screen"))
            {
                customUIManager.FadeToClear();
            }

            GUILayout.EndHorizontal();
        }
    }
#endif

    #endregion
}
