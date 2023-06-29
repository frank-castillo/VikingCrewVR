using System.Collections.Generic;
using UnityEngine;

public class FeedbackHandler : MonoBehaviour
{
    [Header("Standard Feedbacks")]
    [SerializeField] private List<Feedback> _buildUpFeedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onConstantFeedbacks = new List<Feedback>();
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
    private NoteManager _noteManager = null;
    bool _isInitalized = false;

    public void Initialize()
    {
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _noteManager = ServiceLocator.Get<NoteManager>();

        InitializeFeedbacks();
        Subscriptions();

        _isInitalized = true;
    }

    private void Subscriptions()
    {
        _feedbackManager.SubscribeBeatBuildUp(BuildUpFeedack);
        _feedbackManager.SubscribeConstantBeat(OnConstantBeat);
        _feedbackManager.SubscribeOnBeatFirstHit(OnFirstHitFeedback);
        _feedbackManager.SubscribeOnBeatMinorHit(OnMinorHitFeedback);
        _feedbackManager.SubscribeOffBeatMiss(OnMissFeedback);
    }

    private void UnsubscribeMethods()
    {
        _feedbackManager.UnsubscribeBeatBuildUp(BuildUpFeedack);
        _feedbackManager.UnsubscribeConstantBeat(OnConstantBeat);
        _feedbackManager.UnsubscribeOnBeatFirstHit(OnFirstHitFeedback);
        _feedbackManager.UnsubscribeOnBeatMinorHit(OnMinorHitFeedback);
        _feedbackManager.UnsubscribeOffBeatMiss(OnMissFeedback);
    }

    private void InitializeFeedbacks()
    {
        InitializeMiscFeedbacks();
        InitializeFirstHitFeedbacks();
        InitializeMinorHitFeedbacks();
    }

    private void InitializeMiscFeedbacks()
    {
        foreach (var feedback in _buildUpFeedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onConstantFeedbacks)
        {
            feedback.Initialize();
        }

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

    private void BuildUpFeedack()
    {
        PlayFeedbacks(_buildUpFeedbacks);
    }

    private void OnConstantBeat()
    {
        PlayFeedbacks(_onConstantFeedbacks);
    }

    private void OnFirstHitFeedback()
    {
        switch (_noteManager.CurrentTierType)
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
                Enums.InvalidSwitch(GetType(), _noteManager.CurrentTierType.GetType());
                break;
        }
    }

    private void OnMinorHitFeedback()
    {
        switch (_noteManager.CurrentTierType)
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
                Enums.InvalidSwitch(GetType(), _noteManager.CurrentTierType.GetType());
                break;
        }
    }

    private void OnMissFeedback()
    {
        PlayFeedbacks(_onMissFeedbacks);
    }

    private void PlayFeedbacks(List<Feedback> feedbacks)
    {
        foreach (var feedback in feedbacks)
        {
            feedback.Play();
        }
    }
}