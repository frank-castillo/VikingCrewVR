using Liminal.Core.Fader;
using Liminal.SDK.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelLoader : AsyncLoader
{
    [Header("Services")]
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private BeatManager _beatManager = null;
    [SerializeField] private FeedbackManager _feedbackManager = null;

    [Header("Level")]
    [SerializeField] private Ship _ship = null;

    [Header("Fading Times")]
    [SerializeField] private float _fadeInTime = 2.0f;
    [SerializeField] private float _fadeOutTime = 2.0f;

    private static LevelLoader _instance = null;
    private readonly static List<Action> _queuedCallbacks = new List<Action>();

    protected override void Awake()
    {
        LevelSetup();
    }

    // When switching levels we reset the values so they can be overwritten by the new scene and just basic household static cleaning 
    private void OnDestroy()
    {
        ResetVariables();
    }

    private void ProcessQueuedCallbacks()
    {
        foreach (var callback in _queuedCallbacks)
        {
            callback?.Invoke();
        }
    }

    protected override void ResetVariables()
    {
        base.ResetVariables();
        _queuedCallbacks.Clear();
    }

    private void LevelSetup()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        Initialize();

        ProcessQueuedCallbacks();
        CallOnComplete(OnComplete);

        _beatManager.StartBeat();
    }

    private void Initialize()
    {
        _instance = this;
        ServiceLocator.Register<LevelLoader>(this, true);

        // Services
        if (_audioManager != null)
        {
            ServiceLocator.Register<AudioManager>(_audioManager.Initialize(), true);
        }
        if (_beatManager != null)
        {
            ServiceLocator.Register<BeatManager>(_beatManager.Initialize(), true);
        }
        if (_feedbackManager != null)
        {
            ServiceLocator.Register<FeedbackManager>(_feedbackManager.Initialize(), true);
        }

        // Initialize level specific things here
        if (_ship != null)
        {
            _ship.Initialize();
        }

        // Set References
        _beatManager.SetFeedBackManager(_feedbackManager);
    }

    public static void CallOnComplete(Action callback)
    {
        if (_instance == null)
        {
            _queuedCallbacks.Add(callback);
            return;
        }

        _instance.CallOnComplete_Internal(callback);
    }

    private void OnComplete()
    {
        Debug.Log($"<color=Lime> {this.GetType()} finished setup. </color>");

        StartExperience();
    }

    public void StartExperience()
    {
        StartCoroutine(ExperienceFadeIn(_fadeInTime));
    }

    private IEnumerator ExperienceFadeIn(float fadeInTime)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            _audioManager.FadeAudioToStartExperience(elapsedTime / fadeInTime);
            Debug.Log($"<color=Cyan> I am fading in the volume </color>");
            yield return new WaitForEndOfFrame(); // Prevent the loop from finishing in a single frame
        }

        _audioManager.CurrentVolume = 1.0f;
        Debug.Log($"<color=Cyan> I am max volume </color>");
    }

    private IEnumerator ExperienceFadeOut(float fadeOutTime)
    {
        float elapsedTime = 0f;

        ScreenFader.Instance.FadeToBlack(fadeOutTime);

        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _audioManager.FadeAudioToExitExperience(elapsedTime / fadeOutTime);
            yield return new WaitForEndOfFrame(); // Prevent the loop from finishing in a single frame
        }

        _audioManager.CurrentVolume = 0.0f;

        ExperienceApp.End();
    }

    public void FinalizeExperience()
    {
        StartCoroutine(ExperienceFadeOut(_fadeOutTime));
    }

    //TODO: Delete this after tests are done!
    #region Editor

#if UNITY_EDITOR
    [CustomEditor(typeof(LevelLoader))]
    public class LevelLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelLoader customLevelLoader = (LevelLoader)target;

            GUILayout.Space(10F);
            GUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Simulate End of Experience"))
            {
                customLevelLoader.FinalizeExperience();
            }

            if (GUILayout.Button("Simulate Start of Experience"))
            {
                customLevelLoader.StartExperience();
            }

            GUILayout.EndHorizontal();
        }
    }
#endif

    #endregion
}