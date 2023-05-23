using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manifest References")]
    [SerializeField] private AudioManifest _sfxManifest = null;
    [SerializeField] private AudioManifest _splashManifest = null;
    [SerializeField] private AudioManifest _rowingManifest = null;
    [SerializeField] private AudioManifest _chantManifest = null;
    [SerializeField] private AudioManifest _musicManifest = null;

    [Header("Audio Source")]
    [SerializeField] private AudioSource _drumAudioSource = null;
    [SerializeField] private AudioSource _electricDrumAudioSource = null;
    [SerializeField] private AudioSource _vikingAudioSource = null;
    [SerializeField] private AudioSource _leftPaddlesAudioSource = null;
    [SerializeField] private AudioSource _rightPaddlesAudioSource = null;
    [SerializeField] private AudioSource _leftSplashAudioSource = null;
    [SerializeField] private AudioSource _rightSplashAudioSource = null;
    [SerializeField] private AudioSource _musicAudioSource = null;

    // Audio Levels
    private float _currentVolume = 0.0f;

    // Index references
    private int _drumIndex = 1;
    private int _electricIndex = 17;
    private int _rowIndex = 0;
    private int _splashIndex = 0;

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

    public void PlaySFX(SFXType sfxType)
    {
        switch (sfxType)
        {
            case SFXType.OffBeatDrum:
                PlayOffBeatDrumSFX();
                break;
            case SFXType.OnBeatDrum:
                PlayOnBeatDrumSFX();
                PlayDrumElectricHitSFX();
                break;
            case SFXType.PaddleRow:
                PlayPaddleRowSFX();
                break;
            case SFXType.VikingChant:
                PlayVikingChantSFX();
                break;
            case SFXType.SplashLeft:
                PlayLeftSplashSFX();
                break;
            case SFXType.SplashRight:
                PlayRightSplashSFX();
                break;
            case SFXType.DrumHum:
                PlayDrumHumSFX();
                break;
            case SFXType.DrumVacuum:
                PlayDrumVacuumSFX();
                break;
            default:
                Enums.InvalidSwitch(GetType(), sfxType.GetType());
                break;
        }
    }

    private void PlayOffBeatDrumSFX()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[0].Clip);
    }

    private void PlayOnBeatDrumSFX()
    {
        if (_drumIndex >= 14)
        {
            _drumIndex = 1;
        }

        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[_drumIndex].Clip);
        ++_drumIndex;
    }

    private void PlayDrumHumSFX()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[14].Clip);
    }

    private void PlayDrumVacuumSFX()
    {
        _drumAudioSource.PlayOneShot(_sfxManifest.AudioItems[15].Clip);
    }

    private void PlayDrumElectricHitSFX()
    {
        if (_electricIndex >= 20)
        {
            _electricIndex = 17;
        }

        _electricDrumAudioSource.PlayOneShot(_sfxManifest.AudioItems[_electricIndex].Clip);
        ++_electricIndex;
    }

    private void PlayVikingChantSFX()
    {
        // Add counter later
        _vikingAudioSource.PlayOneShot(_chantManifest.AudioItems[0].Clip);
    }

    private void PlayPaddleRowSFX()
    {
        if (_rowIndex >= _rowingManifest.AudioItems.Count)
        {
            _rowIndex = 0;
        }
        _leftPaddlesAudioSource.PlayOneShot(_rowingManifest.AudioItems[_rowIndex].Clip);
        ++_rowIndex;


        if (_rowIndex >= _rowingManifest.AudioItems.Count)
        {
            _rowIndex = 0;
        }
        _rightPaddlesAudioSource.PlayOneShot(_rowingManifest.AudioItems[_rowIndex].Clip);
        ++_rowIndex;
    }

    private void PlayLeftSplashSFX()
    {
        if (_splashIndex >= _splashManifest.AudioItems.Count)
        {
            _splashIndex = 0;
        }

        _leftPaddlesAudioSource.PlayOneShot(_splashManifest.AudioItems[_splashIndex].Clip);
        ++_splashIndex;
    }

    private void PlayRightSplashSFX()
    {
        if (_splashIndex >= _splashManifest.AudioItems.Count)
        {
            _splashIndex = 0;
        }

        _rightSplashAudioSource.PlayOneShot(_splashManifest.AudioItems[_splashIndex].Clip);
        ++_splashIndex;
    }
}