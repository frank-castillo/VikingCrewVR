using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action _constantBeat = null;
    private Action _onBeatHit = null;
    private Action _offBeatMiss = null;

    // Subscribe
    public void ConstantBeatSubscribe(Action action) { _constantBeat += action; }
    public void OnBeatHitSubscribe(Action action) { _onBeatHit += action; }
    public void OffBeatMissSubscribe(Action action) { _offBeatMiss += action; }

    // Unsubscribe
    public void ConstantBeatUnsubscribe(Action action) { _constantBeat -= action; }
    public void OnBeatHitUnsubscribe(Action action) { _onBeatHit -= action; }
    public void OffBeatMissUnsubscribe(Action action) { _offBeatMiss -= action; }

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void ConstantBeatFeedback()
    {
        _constantBeat?.Invoke();
    }

    public void OnBeatFeedback()
    {
        _onBeatHit?.Invoke();
    }

    public void OffBeatFeedback()
    {
        _offBeatMiss?.Invoke();
    }
}