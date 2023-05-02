using System.Collections;
using UnityEngine;


public class ScaleFeedback : Feedback
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _targetScale = Vector3.zero;
    [SerializeField] private AnimationCurve _curve = null;
    [SerializeField] private float _time = 0.0f;

    private Coroutine _coroutine = null;

    public override void Initialize()
    {
        base.Initialize();

        _coroutine = null;
    }

    private IEnumerator ScaleRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        float elapsedTime = 0;
        float timeTakes = _time;
        var cachedEndOfFrame = new WaitForEndOfFrame();
        Transform targetTransform = _target.transform;
        Vector3 initialLocalScale = targetTransform.localScale;

        while (elapsedTime < timeTakes)
        {
            float t = _curve.keys.Length == 0 ? elapsedTime / timeTakes : _curve.Evaluate(elapsedTime / timeTakes);
            targetTransform.localScale = Vector3.LerpUnclamped(initialLocalScale, _targetScale, t);
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
        _coroutine = StartCoroutine(ScaleRoutine());
    }

    public override void Stop()
    {
        if (!isPlaying)
        {
            return;
        }

        base.Stop();
        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}
