using UnityEngine;

public class SoundEffectFeedback : Feedback
{
    [SerializeField] private SFXType _sfxType = SFXType.None;
    private AudioManager _audioManager = null;

    public override void Initialize()
    {
        base.Initialize();
        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    public override void Play()
    {
        base.Play();
        _audioManager.PlaySFX(_sfxType);
    }

    public override void Stop()
    {
        base.Stop();
    }
}
