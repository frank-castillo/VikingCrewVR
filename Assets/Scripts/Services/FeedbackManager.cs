using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    // Both Sides
    private Action _beatBuildUp = null;
    private Action _constantBeat = null;
    private Action _onBeatFirstHit = null;
    private Action _onBeatMinorHit = null;
    private Action _offBeatMiss = null;
    private Action _repeatedMiss = null;

    // Left
    private Action _leftBeatBuildUp = null;
    private Action _leftConstantBeat = null;
    private Action _leftOnBeatFirstHit = null;
    private Action _leftOnBeatMinorHit = null;
    private Action _leftOffBeatMiss = null;
    private Action _leftRepeatedMiss = null;

    // Right
    private Action _rightBeatBuildUp = null;
    private Action _rightConstantBeat = null;
    private Action _rightOnBeatFirstHit = null;
    private Action _rightOnBeatMinorHit = null;
    private Action _rightOffBeatMiss = null;
    private Action _rightRepeatedMiss = null;

    // Subscribe
    public void BeatBuildUpSubscribe(Action action, BeatDirection direction = BeatDirection.Both)
    {
        _beatBuildUp += action;
    }

    public void ConstantBeatSubscribe(Action action) { _constantBeat += action; }
    public void OnBeatFirstHitSubscribe(Action action) { _onBeatFirstHit += action; }
    public void OnBeatMinorHitSubscribe(Action action) { _onBeatMinorHit += action; }
    public void OffBeatMissSubscribe(Action action) { _offBeatMiss += action; }
    public void RepeatedMissSubscribe(Action action) { _repeatedMiss += action; }

    // Unsubscribe
    public void BeatBuildUpUnsubscribe(Action action) { _beatBuildUp -= action; }
    public void ConstantBeatUnsubscribe(Action action) { _constantBeat -= action; }
    public void OnBeatFirstHitUnsubscribe(Action action) { _onBeatFirstHit -= action; }
    public void OnBeatMinorHitUnsubscribe(Action action) { _onBeatMinorHit -= action; }
    public void OffBeatMissUnsubscribe(Action action) { _offBeatMiss -= action; }
    public void RepeatedMissUnsubscribe(Action action) { _repeatedMiss -= action; }

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

    public void RepeatedBeatMiss()
    {
        _repeatedMiss?.Invoke();
    }
}