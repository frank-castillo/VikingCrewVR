using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AsyncLoader : MonoBehaviour
{
    // Sets the importance of a routine and let's us know about it's current progress
    private class RoutineInfo
    {
        public RoutineInfo(IEnumerator routine, int weight, Func<float> progress)
        {
            this.routine = routine;
            this.weight = weight;
            this.progress = progress;
        }

        public readonly IEnumerator routine;
        public readonly int weight;
        public readonly Func<float> progress;
    }

    // Tools for us when initializing any modules
    protected virtual void OnInitComplete() { }
    protected virtual void OnInitUpdate(float percentComplete) { }
    protected virtual void OnInitError(int reasonCode, string reasonDebug) { }

    // Queue of what is pending to be processed
    private Queue<RoutineInfo> _pending = new Queue<RoutineInfo>();
    private bool _completedWithoutError = true;

    // Event invoke OnLoadingCompleted
    protected event Action OnLoadingCompleted;

    protected bool Complete { get; private set; } = false;
    protected float Progress { get; private set; } = 0.0f;

    protected void Enqueue(IEnumerator routine, int weight, Func<float> progress = null)
    {
        _pending.Enqueue(new RoutineInfo(routine, weight, progress));
    }

    protected abstract void Awake();

    private IEnumerator Start()
    {
        if (Complete) // Is the queue complete
        {
            // at 100% double check if this is 1 or 100;
            Progress = 1.0f;
            _pending.Clear();
            yield break;
        }

        // Abstract values we create to keep track of all tasks to be done and measure to define is something has been completed or not
        float percentCompleteByFullSections = 0.0f;
        int outOf = 0;

        // Copy of pending, keep those tasks, free pending for new tasks
        var running = new Queue<RoutineInfo>(_pending);
        _pending.Clear();

        foreach (var routineInfo in running)
        {
            outOf += routineInfo.weight;
        }

        while (running.Count != 0)
        {
            var routineInfo = running.Dequeue();
            var routine = routineInfo.routine;

            while (routine.MoveNext())
            {
                if (routineInfo.progress != null)
                {
                    var routinePercent = routineInfo.progress() * (float)routineInfo.weight / (float)outOf;
                    Progress = percentCompleteByFullSections + routinePercent;
                    OnInitUpdate(Progress);
                }

                yield return routine.Current;
            }

            percentCompleteByFullSections += (float)routineInfo.weight / (float)outOf;
            Progress = percentCompleteByFullSections;
            OnInitUpdate(Progress);
        }

        if (!_completedWithoutError)
        {
            Debug.LogError("A fatal error occurred while running initialization. Please check your logs and fix the error.");
        }

        Complete = true;

        // If Action OnLoadingComplete is null, it means it is empty
        if (OnLoadingCompleted != null)
        {
            OnLoadingCompleted.Invoke();
        }
        else
        {
            Debug.Log("OnComplete Callback is null, possibly in editor for testing use.");
        }
    }

    // Reset all variables. To be used when the game is resetting.
    protected virtual void ResetVariables()
    {
        OnLoadingCompleted = null;
        Complete = false;
        Progress = 0.0f;
    }

    protected void CallOnComplete_Internal(Action callback)
    {
        if (Complete)
        {
            callback();
        }
        else
        {
            OnLoadingCompleted += callback;
        }
    }

    protected void TriggerError(int reasonCode, string reasonDebug)
    {
        Debug.Log(reasonDebug);

        _pending.Clear();
        _completedWithoutError = false;

        OnInitError(reasonCode, reasonDebug);
    }
}
