using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action _constantBeat = null;
    private Action _onBeatHit = null;
    private Action _offBeatMiss = null;
    private Action _repeatedMiss = null;

    // Subscribe
    public void ConstantBeatSubscribe(Action action) { _constantBeat += action; }
    public void OnBeatHitSubscribe(Action action) { _onBeatHit += action; }
    public void OffBeatMissSubscribe(Action action) { _offBeatMiss += action; }
    public void RepeatedMissSubscribe(Action action) { _repeatedMiss += action; }

    // Unsubscribe
    public void ConstantBeatUnsubscribe(Action action) { _constantBeat -= action; }
    public void OnBeatHitUnsubscribe(Action action) { _onBeatHit -= action; }
    public void OffBeatMissUnsubscribe(Action action) { _offBeatMiss -= action; }
    public void RepeatedMissUnsubscribe(Action action) { _repeatedMiss -= action; }

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

    public void RepeatedBeatMiss()
    {
        _repeatedMiss?.Invoke();
    }
}