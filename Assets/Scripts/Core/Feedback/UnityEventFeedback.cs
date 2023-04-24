using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FeedbackSystem
{
    public class UnityEventFeedback : Feedback
    {
        [SerializeField] private UnityEvent onPlay = null;
        private Coroutine _coroutine = null;

        public override void Play()
        {
            if (isPlaying)
            {
                return;
            }

            base.Play();
            onPlay?.Invoke();
            _coroutine = StartCoroutine(DelayedInvokeRoutine());
        }

        private IEnumerator DelayedInvokeRoutine()
        {
            yield return new WaitForSeconds(initialDelay);
            onPlay?.Invoke();
            isPlaying = false;
        }
    }
}