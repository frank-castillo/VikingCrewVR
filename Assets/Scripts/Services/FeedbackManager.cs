using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action<BeatDirection> _beatBuildUp = null;
    private Action<BeatDirection> _constantBeat = null;
    private Action<BeatDirection> _onBeatFirstHit = null;
    private Action<BeatDirection> _onBeatMinorHit = null;
    private Action<BeatDirection> _offBeatMiss = null;
    private Action<BeatDirection> _repeatedMiss = null;

    // Subscribe
    public void BeatBuildUpSubscribe(Action<BeatDirection> action) { _beatBuildUp += action; }
    public void ConstantBeatSubscribe(Action<BeatDirection> action) { _constantBeat += action; }
    public void OnBeatFirstHitSubscribe(Action<BeatDirection> action) { _onBeatFirstHit += action; }
    public void OnBeatMinorHitSubscribe(Action<BeatDirection> action) { _onBeatMinorHit += action; }
    public void OffBeatMissSubscribe(Action<BeatDirection> action) { _offBeatMiss += action; }
    public void RepeatedMissSubscribe(Action<BeatDirection> action) { _repeatedMiss += action; }

    // Unsubscribe
    public void BeatBuildUpUnsubscribe(Action<BeatDirection> action) { _beatBuildUp -= action; }
    public void ConstantBeatUnsubscribe(Action<BeatDirection> action) { _constantBeat -= action; }
    public void OnBeatFirstHitUnsubscribe(Action<BeatDirection> action) { _onBeatFirstHit -= action; }
    public void OnBeatMinorHitUnsubscribe(Action<BeatDirection> action) { _onBeatMinorHit -= action; }
    public void OffBeatMissUnsubscribe(Action<BeatDirection> action) { _offBeatMiss -= action; }
    public void RepeatedMissUnsubscribe(Action<BeatDirection> action) { _repeatedMiss -= action; }

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

    public void RepeatedBeatMiss(BeatDirection direction)
    {
        _repeatedMiss?.Invoke(direction);
    }
}