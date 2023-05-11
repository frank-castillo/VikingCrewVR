using UnityEngine;

public class AnimationFeedback : Feedback
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private string _triggerName = string.Empty;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Play()
    {
        base.Play();

        _animator.SetTrigger(_triggerName);
    }

    public override void Stop()
    {
        base.Stop();
    }
}
