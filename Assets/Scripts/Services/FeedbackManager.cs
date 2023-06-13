using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action _beatBuildUp = null;
    private Action _constantBeat = null;
    private Action _onBeatFirstHit = null;
    private Action _onBeatMinorHit = null;
    private Action _offBeatMiss = null;

    // Subscribe
    public void SubscribeBeatBuildUp(Action action) { _beatBuildUp += action; }
    public void SubscribeConstantBeat(Action action) { _constantBeat += action; }
    public void SubscribeOnBeatFirstHit(Action action) { _onBeatFirstHit += action; }
    public void SubscribeOnBeatMinorHit(Action action) { _onBeatMinorHit += action; }
    public void SubscribeOffBeatMiss(Action action) { _offBeatMiss += action; }

    // Unsubscribe
    public void UnsubscribeBeatBuildUp(Action action) { _beatBuildUp -= action; }
    public void UnsubscribeConstantBeat(Action action) { _constantBeat -= action; }
    public void UnsubscribeOnBeatFirstHit(Action action) { _onBeatFirstHit -= action; }
    public void UnsubscribeOnBeatMinorHit(Action action) { _onBeatMinorHit -= action; }
    public void UnsubscribeOffBeatMiss(Action action) { _offBeatMiss -= action; }

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void BeatBuildUpFeedback()
    {
        _beatBuildUp?.Invoke();
    }

    public void ConstantBeatFeedback()
    {
        _constantBeat?.Invoke();
    }

    public void OnFirstBeatFeedback()
    {
        _onBeatFirstHit?.Invoke();
        _onBeatMinorHit?.Invoke();
    }

    public void OnMinorBeatFeedback()
    {
        _onBeatMinorHit?.Invoke();
    }

    public void OffBeatFeedback()
    {
        _offBeatMiss?.Invoke();
    }
}