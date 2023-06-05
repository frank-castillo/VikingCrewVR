using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action<BeatDirection> _beatBuildUp = null;
    private Action<BeatDirection> _constantBeat = null;
    private Action<BeatDirection> _onBeatFirstHit = null;
    private Action<BeatDirection> _onBeatMinorHit = null;
    private Action<BeatDirection> _offBeatMiss = null;

    // Subscribe
    public void SubscribeBeatBuildUp(Action<BeatDirection> action) { _beatBuildUp += action; }
    public void SubscribeConstantBeat(Action<BeatDirection> action) { _constantBeat += action; }
    public void SubscribeOnBeatFirstHit(Action<BeatDirection> action) { _onBeatFirstHit += action; }
    public void SubscribeOnBeatMinorHit(Action<BeatDirection> action) { _onBeatMinorHit += action; }
    public void SubscribeOffBeatMiss(Action<BeatDirection> action) { _offBeatMiss += action; }

    // Unsubscribe
    public void UnsubscribeBeatBuildUp(Action<BeatDirection> action) { _beatBuildUp -= action; }
    public void UnsubscribeConstantBeat(Action<BeatDirection> action) { _constantBeat -= action; }
    public void UnsubscribeOnBeatFirstHit(Action<BeatDirection> action) { _onBeatFirstHit -= action; }
    public void UnsubscribeOnBeatMinorHit(Action<BeatDirection> action) { _onBeatMinorHit -= action; }
    public void UnsubscribeOffBeatMiss(Action<BeatDirection> action) { _offBeatMiss -= action; }

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void BeatBuildUpFeedback(BeatDirection direction)
    {
        _beatBuildUp?.Invoke(direction);
    }

    public void ConstantBeatFeedback(BeatDirection direction)
    {
        _constantBeat?.Invoke(direction);
    }

    public void OnFirstBeatFeedback(BeatDirection direction)
    {
        _onBeatFirstHit?.Invoke(direction);
        _onBeatMinorHit?.Invoke(direction);
    }

    public void OnMinorBeatFeedback(BeatDirection direction)
    {
        _onBeatMinorHit?.Invoke(direction);
    }

    public void OffBeatFeedback(BeatDirection direction)
    {
        _offBeatMiss?.Invoke(direction);
    }
}