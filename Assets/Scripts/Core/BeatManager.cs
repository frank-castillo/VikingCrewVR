﻿using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _hitWindowDelay = 0.2f; // On either side

    [Header("Combo Tiers")]
    [SerializeField] private int _tierTwo = 0;
    [SerializeField] private int _tierThree = 0;

    private FeedbackManager _feedbackManager = null;
    private HammerController _leftHammerController = null;
    private HammerController _rightHammerController = null;
    private OVRInput.Controller _activeController = OVRInput.Controller.None;

    private int _beatStreak = 0;
    private float _beatTimer = 0.0f;
    private float _hitWindowTimer = 0.0f;
    private bool _isOnBeat = false;
    private bool _isPlaying = false;

    public OVRInput.Controller ActiveController { get => _activeController; }

    public int TierTwo { get => _tierTwo; }
    public int TierThree { get => _tierThree; }

    public void SetActiveController(OVRInput.Controller controller) { _activeController = controller; }
    public void SetFeedBackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }
    public void SetLeftHammerController(HammerController leftHammerController) { _leftHammerController = leftHammerController; }
    public void SetRightHammerController(HammerController rightHammerController) { _rightHammerController = rightHammerController; }

    public void StopBeat() { _isPlaying = false; }

    public void StartBeat()
    {
        _isPlaying = true;
        _beatTimer = _beatDelay;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    private void Update()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _beatTimer -= Time.deltaTime;
        if (_beatTimer < 0)
        {
            Beat();
        }
        else if (_isOnBeat)
        {
            EvaluateHitWindow();
        }
    }

    private void Beat()
    {
        _isOnBeat = true;

        _feedbackManager.ConstantBeatFeedback();

        _beatTimer = _beatDelay;
        _hitWindowTimer = _hitWindowDelay;
    }

    private void EvaluateHitWindow()
    {
        _hitWindowTimer -= Time.deltaTime;

        if (_hitWindowTimer < 0)
        {
            _isOnBeat = false;
        }
    }

    public void DrumHit()
    {
        if (_isOnBeat || PreBeatCheck())
        {
            HitOnBeat();
        }
        else
        {
            HitOffBeat();
        }

        _leftHammerController.LevelEvaluation(_beatStreak);
        _rightHammerController.LevelEvaluation(_beatStreak);
    }

    private void HitOnBeat()
    {
        ++_beatStreak;
        _feedbackManager.OnBeatFeedback(EvaluateStreak());
    }

    private void HitOffBeat()
    {
        _feedbackManager.OffBeatFeedback();
    }

    private bool PreBeatCheck()
    {
        float delta = _beatTimer - _hitWindowDelay;
        return delta < 0;
    }

    private OnHitBeatType EvaluateStreak()
    {
        if (_beatStreak < _tierTwo)
        {
            return OnHitBeatType.T1;
        }
        else if (_beatStreak < _tierThree)
        {
            return OnHitBeatType.T2;
        }
        else
        {
            return OnHitBeatType.T3;
        }
    }
}