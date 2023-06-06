using Liminal.Core.Fader;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIManager : MonoBehaviour
{
    [SerializeField] private float _fadeInTime = 2.0f;
    [SerializeField] private float _fadeOutTime = 2.0f;
    public UIManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }
    public void FadeInScreen()
    {
        ScreenFader.Instance.FadeToClearFromBlack(_fadeInTime);
    }

    public void FadeOutScreen()
    {
        ScreenFader.Instance.FadeToBlack(_fadeOutTime);
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
                customUIManager.FadeOutScreen();
            }

            if (GUILayout.Button("Fade In Screen"))
            {
                customUIManager.FadeInScreen();
            }

            GUILayout.EndHorizontal();
        }
    }
#endif

    #endregion
}
