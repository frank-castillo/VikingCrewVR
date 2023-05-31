using System.Collections.Generic;
using UnityEngine;

public class FeedbackHandler : MonoBehaviour
{
    [Header("Misc Feedbacks")]
    [SerializeField] private List<Feedback> _onConstantBeatFeedbacks = new List<Feedback>();
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

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _beatManager = ServiceLocator.Get<BeatManager>();

        InitializeFeedbacks();
        Subscriptions();
    }

    private void Subscriptions()
    {
        _feedbackManager.ConstantBeatSubscribe(ConstantBeatFeedback);
        _feedbackManager.OnBeatFirstHitSubscribe(OnFirstHitFeedback);
        _feedbackManager.OnBeatMinorHitSubscribe(OnMinorHitFeedback);
        _feedbackManager.OffBeatMissSubscribe(OnMissFeedback);
    }

    private void UnsubscribeMethods()
    {
        _feedbackManager.ConstantBeatUnsubscribe(ConstantBeatFeedback);
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
        foreach (var feedback in _onConstantBeatFeedbacks)
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
        UnsubscribeMethods();
    }

    private void ConstantBeatFeedback(BeatDirection beatDirection)
    {
        PlayFeedbacks(_onConstantBeatFeedbacks);
    }

    private void OnFirstHitFeedback(BeatDirection beatDirection)
    {
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