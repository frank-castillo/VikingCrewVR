using System.Collections.Generic;
using UnityEngine;

public class FeedbackHandler : MonoBehaviour
{
    [Header("Feedback References")]
    [SerializeField] private List<Feedback> _onConstantBeatFeedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onMissFeedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onBeatT1Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onBeatT2Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onBeatT3Feedbacks = new List<Feedback>();
    [SerializeField] private List<Feedback> _onAnyBeatTierFeedbacks = new List<Feedback>();

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
        _feedbackManager.OnBeatHitSubscribe(OnHitFeedback);
        _feedbackManager.OffBeatMissSubscribe(OnMissFeedback);
    }

    private void UnsubscribeMethods()
    {
        _feedbackManager.ConstantBeatUnsubscribe(ConstantBeatFeedback);
        _feedbackManager.OnBeatHitUnsubscribe(OnHitFeedback);
        _feedbackManager.OffBeatMissUnsubscribe(OnMissFeedback);
    }

    private void InitializeFeedbacks()
    {
        foreach (var feedback in _onConstantBeatFeedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onMissFeedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onBeatT1Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onBeatT2Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onBeatT3Feedbacks)
        {
            feedback.Initialize();
        }

        foreach (var feedback in _onAnyBeatTierFeedbacks)
        {
            feedback.Initialize();
        }
    }

    private void OnDestroy()
    {
        UnsubscribeMethods();
    }

    private void ConstantBeatFeedback()
    {
        PlayFeedbacks(_onConstantBeatFeedbacks);
    }

    private void OnHitFeedback()
    {
        PlayFeedbacks(_onAnyBeatTierFeedbacks);

        switch (_beatManager.CurrentTier)
        {
            case BeatTierType.None:
                break;
            case BeatTierType.T1:
                PlayFeedbacks(_onBeatT1Feedbacks);
                break;
            case BeatTierType.T2:
                PlayFeedbacks(_onBeatT2Feedbacks);
                break;
            case BeatTierType.T3:
                PlayFeedbacks(_onBeatT3Feedbacks);
                break;
            default:
                Enums.InvalidSwitch(GetType(), _beatManager.CurrentTier.GetType());
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