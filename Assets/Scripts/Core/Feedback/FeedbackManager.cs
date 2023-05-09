using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    private Action _constantBeat = null;
    private Action _onBeatHit = null;
    private Action _offBeatMiss = null;

    public void ConstantBeatSubcribe(Action action) { _constantBeat += action; }
    public void OnBeatHitSubscribe(Action action) { _onBeatHit += action; }
    public void OffBeatMissSubcribe(Action action) { _offBeatMiss += action; }

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

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