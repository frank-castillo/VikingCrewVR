﻿using System.Collections;
using UnityEngine;

public class HapticFeedback : Feedback
{
    [SerializeField] private float _intensity = 0.0f;

    private BeatManager _beatManager = null;
    private Coroutine _coroutine = null;

    public override void Initialize()
    {
        base.Initialize();

        _beatManager = ServiceLocator.Get<BeatManager>();
        _coroutine = null;
    }

    private IEnumerator HapticFeedbackRoutine()
    {
        OVRInput.SetControllerVibration(1, _intensity, _beatManager.ActiveController);

        yield return new WaitForSecondsRealtime(0.1f);

        OVRInput.SetControllerVibration(0, 0, _beatManager.ActiveController);

        isPlaying = false;
    }

    public override void Play()
    {
        //if (isPlaying)
        //{
        //    return;
        //}

        base.Play();

        _coroutine = StartCoroutine(HapticFeedbackRoutine());
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