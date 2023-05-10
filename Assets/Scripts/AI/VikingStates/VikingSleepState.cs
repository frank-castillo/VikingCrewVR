using UnityEngine;

public class VikingSleepState : IVikingState
{
    private VikingBehavior _behavior = null;

    private bool _timerActive = false;
    private float _timer = 0.0f;
    private float _delay = 0.0f;
    private float _variance = 0.0f;

    public VikingSleepState(VikingBehavior vikingBehavior, float delay, float variance)
    {
        _behavior = vikingBehavior;
        _delay = delay;
        _variance = variance;
    }

    public void Enter()
    {
        _behavior.ChangeAnimation(VikingAnimationType.Idle);

        StartTimer();
    }

    public void Update()
    {
        if (_timerActive == false)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer < 0.0f)
        {
            RandomAnimation();
        }
    }

    public void Exit()
    {
        _timerActive = false;
    }

    private void StartTimer()
    {
        _timer = _delay + Random.Range(-_variance, _variance);
        _timerActive = true;
    }

    private void RandomAnimation()
    {
        VikingAnimationType currentAnim = _behavior.CurrentAnimationType;
        if (currentAnim != VikingAnimationType.Idle)
        {
            _behavior.ChangeAnimation(VikingAnimationType.Idle);
        }
        else
        {
            _behavior.ChangeAnimation(GetRandomNewAnimation(currentAnim));
        }

        StartTimer();
    }

    private VikingAnimationType GetRandomNewAnimation(VikingAnimationType currentAnim)
    {
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            return VikingAnimationType.Stretch;
        }
        else
        {
            return VikingAnimationType.Yawn;
        }
    }
}