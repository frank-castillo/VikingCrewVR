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

    [Header("Time Scale")]
    [SerializeField] private float timeScale = 1f;

    [Header("Fading Times")]
    [SerializeField] private float _fadeInTime = 2.0f;
    [SerializeField] private float _fadeOutTime = 2.0f;

    [Header("Wrap Up Time")]
    [SerializeField] private float _wrapUpDelay = 0.0f;
    private float _gameTimer = 0.0f;
    private bool _gameStarted = false;
    private bool _wrapUpStarted = false;

    [Header("References")]
    [SerializeField] private Ship _ship = null;
    [SerializeField] private EnvironmentManager _environment = null;
    [SerializeField] private HammerController _leftHammer = null;
    [SerializeField] private HammerController _rightHammer = null;

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
        SetupEvents();

        ProcessQueuedCallbacks();
        CallOnComplete(OnComplete);

        SetupSceneStart();
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

        if (_environment != null)
        {
            _environment.Initialize();
        }

        if (_leftHammer != null && _rightHammer != null)
        {
            _leftHammer.Initialize();
            _rightHammer.Initialize();
        }

        // Set References
        _beatManager.SetFeedBackManager(_feedbackManager);
    }

    private void Update()
    {
        if (_gameStarted == false || _wrapUpStarted == false)
        {
            return;
        }

        _gameTimer += Time.deltaTime;
        if (_gameTimer > _wrapUpDelay)
        {
            WrapUpSequence();
        }

        //Time.timeScale = timeScale;
    }

    private void SetupEvents()
    {
        _feedbackManager.OnBeatFirstHitSubscribe(_leftHammer.LevelEvaluation);
        _feedbackManager.OnBeatFirstHitSubscribe(_rightHammer.LevelEvaluation);
        _feedbackManager.RepeatedMissSubscribe(_leftHammer.LevelEvaluation);
        _feedbackManager.RepeatedMissSubscribe(_rightHammer.LevelEvaluation);
    }

    private void UnsubscribeEvents()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(_leftHammer.LevelEvaluation);
        _feedbackManager.OnBeatFirstHitUnsubscribe(_rightHammer.LevelEvaluation);
        _feedbackManager.RepeatedMissUnsubscribe(_leftHammer.LevelEvaluation);
        _feedbackManager.RepeatedMissUnsubscribe(_rightHammer.LevelEvaluation);
    }

    private void SetupSceneStart()
    {
        _beatManager.StartBeat();
        _environment.StartEnvironment();

        _gameStarted = true;
    }

    private void WrapUpSequence()
    {
        _wrapUpStarted = true;

        _ship.ShipWrapUp();
        _environment.EnvironmentWrapUp();
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
            //Debug.Log($"<color=Cyan> I am fading in the volume </color>");
            yield return new WaitForEndOfFrame(); // Prevent the loop from finishing in a single frame
        }

        _audioManager.CurrentVolume = 1.0f;
        //Debug.Log($"<color=Cyan> I am max volume </color>");
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

        UnsubscribeEvents();

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
                customLevelLoader.WrapUpSequence();
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