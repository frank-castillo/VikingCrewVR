public class OnMissPlayDrumSoundFeedback : Feedback
{
    private AudioManager _audioManager = null;

    public override void Initialize()
    {
        base.Initialize();

        _audioManager = ServiceLocator.Get<AudioManager>();
    }

    public override void Play()
    {
        base.Play();
        _audioManager.PlayOffBeatDrumSound();
    }

    public override void Stop()
    {
        base.Stop();
    }
}
