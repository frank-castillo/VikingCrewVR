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

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        Subscriptions();
    }

    private void Subscriptions()
    {
        _feedbackManager.constantBeat += ConstantBeatFeedback;
        _feedbackManager.onBeatHit += OnHitFeedback;
        _feedbackManager.offBeatMiss += OnMissFeedback;
    }

    private void ConstantBeatFeedback()
    {
        PlayFeedbacks(_onConstantBeatFeedbacks);
    }

    private void OnHitFeedback(OnHitBeatType beatType)
    {
        PlayFeedbacks(_onAnyBeatTierFeedbacks);

        switch (beatType)
        {
            case OnHitBeatType.None:
                break;
            case OnHitBeatType.T1:
                PlayFeedbacks(_onBeatT1Feedbacks);
                break;
            case OnHitBeatType.T2:
                PlayFeedbacks(_onBeatT2Feedbacks);
                break;
            case OnHitBeatType.T3:
                PlayFeedbacks(_onBeatT3Feedbacks);
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatType.GetType());
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