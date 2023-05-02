using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    [SerializeField] protected float initialDelay = 0.0f;
    protected bool isPlaying = false;

    public virtual void Initialize()
    {
        isPlaying = false;
    }

    public virtual void Play()
    {
        isPlaying = true;
    }

    public virtual void Stop()
    {
        isPlaying = false;
    }
}
