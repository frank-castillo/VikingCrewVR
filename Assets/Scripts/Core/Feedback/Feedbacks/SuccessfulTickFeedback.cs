using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuccessfulTickFeedback : Feedback
{
    [SerializeField] private UnityEvent _onPlay = null;
    [SerializeField] private int _successCounter = 1;
    [SerializeField] private List<Feedback> _wrappedFeedbacks = new List<Feedback>();

    private Coroutine _coroutine = null;
    private int _currentStreak = 0;

    public override void Play()
    {
        if (isPlaying)
        {
            return;
        }

        ++_currentStreak;

        if (_currentStreak == _successCounter)
        {
            base.Play();

            foreach (var feedback in _wrappedFeedbacks)
            {
                feedback.Play();
            }
            _onPlay?.Invoke();
            _coroutine = StartCoroutine(DelayedInvokeRoutine());
            _currentStreak = 0;
        }
    }

    private IEnumerator DelayedInvokeRoutine()
    {
        yield return new WaitForSeconds(initialDelay);
        _onPlay?.Invoke();
        isPlaying = false;
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

    public override void Initialize()
    {
        base.Initialize();
        foreach (var feedback in _wrappedFeedbacks)
        {
            feedback.Initialize();
        }
    }
}