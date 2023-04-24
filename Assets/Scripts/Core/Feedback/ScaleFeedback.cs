using System.Collections;
using UnityEngine;

namespace FeedbackSystem
{
    public class ScaleFeedback : Feedback
    {
        [SerializeField] private Transform _target = null;
        [SerializeField] private Vector3 _targetScale = Vector3.zero;
        [SerializeField] private AnimationCurve _curve = null;
        [SerializeField] private float _time = 0.0f;

        // Potential functionality
        //[SerializeField] private bool _remapOnOne = false;
        //[SerializeField] private bool _remapOnZero = false;
        //[SerializeField] private float _speed = false;

        private Vector3 _initialScale = Vector3.zero;
        private Coroutine _coroutine = null;

        public override void Initialize()
        {
            base.Initialize();

            _initialScale = _target.transform.localScale;
            _coroutine = null;
        }

        private IEnumerator ScaleRoutuine()
        {
            yield return new WaitForSeconds(initialDelay);

            float elapsedTime = 0;
            float timeTakes = _time;
            var cachedEndOfFrame = new WaitForEndOfFrame();
            _target.transform.localScale = _initialScale;

            while (elapsedTime < timeTakes)
            {
                float t = _curve.keys.Length == 0 ? elapsedTime / timeTakes : _curve.Evaluate(elapsedTime / timeTakes);
                _target.localScale = Vector3.LerpUnclamped(_initialScale, _targetScale, t);
                elapsedTime += Time.deltaTime;
                yield return cachedEndOfFrame;
            }
            isPlaying = false;
        }
     
        public override void Play()
        {
            if (isPlaying)
            {
                return;
            }

            base.Play();
            _coroutine = StartCoroutine(ScaleRoutuine());
        }

    }
}