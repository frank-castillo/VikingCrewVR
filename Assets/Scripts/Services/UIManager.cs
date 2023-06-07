using Liminal.Core.Fader;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UIManager : MonoBehaviour
{
    [SerializeField] private const float _defaultFadeTime = 2.0f;
    [SerializeField] private GameObject _difficultySelectionUI = null;

    public UIManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");
        _difficultySelectionUI.SetActive(true);

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

    public void TurnOffDifficultySelection()
    {
        _difficultySelectionUI.SetActive(false);
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
