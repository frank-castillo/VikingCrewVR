﻿using System.Collections.Generic;
using UnityEngine;

public class DrumResponseFeedbackHandler : MonoBehaviour
{
    [Header("Drum Info")]
    private DrumSide _drumSide = DrumSide.None;

    [Header("Misc Feedbacks")]
    [SerializeField] private List<Feedback> _onMissFeedbacks = new List<Feedback>();

    [Header("First Hit")]
    [SerializeField] private List<Feedback> _onFirstBeatT1Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onFirstBeatT2Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onFirstBeatT3Feedbacks = new List<Feedback>();

    [Header("Minor Hit")]
    [SerializeField] private List<Feedback> _onMinorBeatT1Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onMinorBeatT2Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onMinorBeatT3Feedbacks = new List<Feedback>();

    private FeedbackManager _feedbackManager = null;
    private BeatManager _beatManager = null;
    bool _isInitalized = false;

    public void Initialize(DrumSide drumSide)
    {
        _drumSide = drumSide;

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _beatManager = ServiceLocator.Get<BeatManager>();

        InitializeFeedbacks();
        Subscriptions();

        _isInitalized = true;
    }

    private void Subscriptions()
    {
        _feedbackManager.OnBeatFirstHitSubscribe(OnFirstHitFeedback);
        _feedbackManager.OnBeatMinorHitSubscribe(OnMinorHitFeedback);
        _feedbackManager.OffBeatMissSubscribe(OnMissFeedback);
    }

    private void UnsubscribeMethods()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(OnFirstHitFeedback);
        _feedbackManager.OnBeatMinorHitUnsubscribe(OnMinorHitFeedback);
        _feedbackManager.OffBeatMissUnsubscribe(OnMissFeedback);
    }

    private void InitializeFeedbacks()
    {
        InitializeMiscFeedbacks();
        InitializeFirstHitFeedbacks();
        InitializeMinorHitFeedbacks();
    }

    private void InitializeMiscFeedbacks()
    {
        foreach (var feedback in _onMissFeedbacks)
        {
            feedback.Initialize();
        }
    }

    private void InitializeFirstHitFeedbacks()
    {
        foreach (var feedback in _onFirstBeatT1Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onFirstBeatT2Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onFirstBeatT3Feedbacks)
        {
            feedback.Initialize();
        }
    }

    private void InitializeMinorHitFeedbacks()
    {
        foreach (var feedback in _onMinorBeatT1Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onMinorBeatT2Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onMinorBeatT3Feedbacks)
        {
            feedback.Initialize();
        }
    }

    private void OnDestroy()
    {
        if (_isInitalized == false)
        {
            Debug.LogError($"{GetType()} on {gameObject.name} is calling OnDestroy but is never Initialized");
            return;
        }

        UnsubscribeMethods();
    }

    private void OnFirstHitFeedback(BeatDirection beatDirection)
    {
        if (IsCorrectDrum(beatDirection) == false)
        {
            return;
        }

        switch (_beatManager.CurrentTier)
        {
            case BeatTierType.None:
                break;
            case BeatTierType.T1:
                PlayFeedbacks(_onFirstBeatT1Feedbacks);
                break;
            case BeatTierType.T2:
                PlayFeedbacks(_onFirstBeatT1Feedbacks);
                PlayFeedbacks(_onFirstBeatT2Feedbacks);
                break;
            case BeatTierType.T3:
                PlayFeedbacks(_onFirstBeatT1Feedbacks);
                PlayFeedbacks(_onFirstBeatT2Feedbacks);
                PlayFeedbacks(_onFirstBeatT3Feedbacks);
                break;
            default:
                Enums.InvalidSwitch(GetType(), _beatManager.CurrentTier.GetType());
                break;
        }
    }

    private void OnMinorHitFeedback(BeatDirection beatDirection)
    {
        if (IsCorrectDrum(beatDirection) == false)
        {
            return;
        }

        switch (_beatManager.CurrentTier)
        {
            case BeatTierType.None:
                break;
            case BeatTierType.T1:
                PlayFeedbacks(_onMinorBeatT1Feedbacks);
                break;
            case BeatTierType.T2:
                PlayFeedbacks(_onMinorBeatT1Feedbacks);
                PlayFeedbacks(_onMinorBeatT2Feedbacks);
                break;
            case BeatTierType.T3:
                PlayFeedbacks(_onMinorBeatT1Feedbacks);
                PlayFeedbacks(_onMinorBeatT2Feedbacks);
                PlayFeedbacks(_onMinorBeatT3Feedbacks);
                break;
            default:
                Enums.InvalidSwitch(GetType(), _beatManager.CurrentTier.GetType());
                break;
        }
    }

    private void OnMissFeedback(BeatDirection beatDirection)
    {
        if (IsCorrectDrum(beatDirection) == false)
        {
            return;
        }

        PlayFeedbacks(_onMissFeedbacks);
    }

    private void PlayFeedbacks(List<Feedback> feedbacks)
    {
        foreach (var feedback in feedbacks)
        {
            feedback.Play();
        }
    }

    private bool IsCorrectDrum(BeatDirection beatDirection)
    {
        switch (beatDirection)
        {
            case BeatDirection.Left:
                if (_drumSide == DrumSide.Left)
                {
                    return true;
                }
                return false;
            case BeatDirection.Right:
                if (_drumSide == DrumSide.Right)
                {
                    return true;
                }
                return false;
            case BeatDirection.Both:
                return false;
            default:
                Enums.InvalidSwitch(GetType(), beatDirection.GetType());
                return false;
        }
    }
}