using UnityEngine;
using System;

public class FeedbackManager : MonoBehaviour
{
    public Action onBeat = null;
    public Action<int> onBeatHit = null;
    public Action offBeatMiss = null;

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void StandardBeatFeedback()
    {
        onBeat?.Invoke();
    }

    public void OnBeatFeedback(int streak)
    {
        onBeatHit?.Invoke(streak);
    }

    public void OffBeatFeedback()
    {
        offBeatMiss?.Invoke();
    }
}