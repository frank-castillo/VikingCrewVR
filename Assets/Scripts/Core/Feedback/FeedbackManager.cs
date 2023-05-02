using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    public Action constantBeat = null;
    public Action<OnHitBeatType> onBeatHit = null;
    public Action offBeatMiss = null;

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void ConstantBeatFeedback()
    {
        constantBeat?.Invoke();
    }

    public void OnBeatFeedback(OnHitBeatType beatType)
    {
        onBeatHit?.Invoke(beatType);
    }

    public void OffBeatFeedback()
    {
        offBeatMiss?.Invoke();
    }
}