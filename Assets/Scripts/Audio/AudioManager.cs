using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manifest References")]
    [SerializeField] private AudioManifest _sfxManifest = null;
    [SerializeField] private AudioManifest _musicManifest = null;

    [Header("Audio Source")]
    [SerializeField] private AudioSource _drumAudioSource = null;
    [SerializeField] private AudioSource _vikingAudioSource = null;
    [SerializeField] private AudioSource _musicAudioSource = null;

    // Audio Levels
    float _currentVolume = 0.0f;

    // Properties
    public float CurrentVolume { set => _currentVolume = value; }

    public AudioManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void FadeAudioToExitExperience(float fadeOutTime)
    {
        AudioListener.volume = Mathf.Lerp(_currentVolume, 0f, fadeOutTime);
    }

    public void FadeAudioToStartExperience(float fadeInTime)
    {
        AudioListener.volume = Mathf.Lerp(_currentVolume, 1.0f, fadeInTime);
    }

    public void PlayMusic()
    {
        _musicAudioSource.PlayOneShot(_musicManifest.AudioItems[0].Clip);
    }

    public void PlayOffBeatDrumSound()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[0].Clip);
    }

    public void PlayOnBeatDrumSound()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[1].Clip);
    }

    public void PlayVikingChant()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[2].Clip);
    }
}