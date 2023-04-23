using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : AsyncLoader
{
    [Header("Services")]
    [SerializeField] private InputManager _inputManager = null;
    [SerializeField] private AudioManager _audioManager = null;

    [Header("Level")]
    [SerializeField] private BeatManager _beatManager = null;
    [SerializeField] private Ship _ship = null;

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

    private void Initialize()
    {
        _instance = this;
        ServiceLocator.Register<LevelLoader>(this, true);

        // Services
        if (_inputManager != null)
        {
            ServiceLocator.Register<InputManager>(_inputManager.Initialize(), true);
        }

        if (_audioManager != null)
        {
            ServiceLocator.Register<AudioManager>(_audioManager.Initialize(), true);
        }

        // Initialize level specific things here
        if (_beatManager != null)
        {
            _beatManager.Initialize();
        }
        if (_ship != null)
        {
            _ship.Initialize();
        }
    }

    private void LevelSetup()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        Initialize();

        ProcessQueuedCallbacks();
        CallOnComplete(OnComplete);
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
    }
}