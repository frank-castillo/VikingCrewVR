using UnityEngine;

namespace FeedbackSystem
{
    public abstract class Feedback : MonoBehaviour, IPlayable
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
    }
}
