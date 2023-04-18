using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : AsyncLoader
{
    private static LevelLoader _instance = null;
    private readonly static List<Action> _queuedCallbacks = new List<Action>();

    [SerializeField] private Enums.SceneType _sceneType = Enums.SceneType.None;
    [SerializeField] private GameEvent onLevelLoadedEvent = null;
    
    public Enums.SceneType SceneType { get => _sceneType; }

    [Serializable]
    public class DummyDictionary
    {
        public System.Type Key;
        public GameObject Value;
    }
    // [SerializeField] private List<DummyDictionary> _dummyDict = new List<DummyDictionary>();

    protected override void Awake()
    {
        GameLoader.CallOnComplete(LevelSetup);
    }

    // When switching levels we reset the values so they can be overwritten by the new scene and just basic household static cleaning 
    private void OnDestroy()
    {
        ResetVariables();
    }

    public Enums.SceneType GetSceneType()
    {
        return _sceneType;
    }

    private void LevelSetup()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");
        Initialize();
        ProcessQueuedCallbacks();
        CallOnComplete(OnComplete);
    }

    private void Initialize()
    {
        _instance = this;
        ServiceLocator.Register<LevelLoader>(this, true);

        // Initialize level specific things here
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
        onLevelLoadedEvent?.Invoke();
    }

}