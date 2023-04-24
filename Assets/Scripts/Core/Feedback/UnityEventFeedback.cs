using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace FeedbackSystem
{
    public class UnityEventFeedback : Feedback
    {
        [SerializeField] private UnityEvent _onPlay = null;
        private Coroutine _coroutine = null;

        public override void Play()
        {
            if (isPlaying)
            {
                return;
            }

            base.Play();
            _onPlay?.Invoke();
            _coroutine = StartCoroutine(DelayedInvokeRoutine());
        }

        private IEnumerator DelayedInvokeRoutine()
        {
            yield return new WaitForSeconds(initialDelay);
            _onPlay?.Invoke();
            isPlaying = false;
        }
    }
}