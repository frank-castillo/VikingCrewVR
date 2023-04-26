using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _hitWindowDelay = 0.2f; // On either side

    [Header("References")]
    [SerializeField] private FeedbackManager _feedbackManager = null;

    private AudioManager _audioManager = null;

    private int _beatStreak = 0;

    private float _beatTimer = 0.0f;
    private float _hitWindowTimer = 0.0f;
    private bool _isOnBeat = false;
    private bool _isPlaying = false;

    public void StopBeat() { _isPlaying = false; }

    public void StartBeat()
    {
        _isPlaying = true;
        _beatTimer = _beatDelay;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager.Initialize();

        _audioManager = ServiceLocator.Get<AudioManager>();

        return this;
    }

    private void Update()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _beatTimer -= Time.deltaTime;
        if (_beatTimer < 0)
        {
            Beat();
        }

        if (_isOnBeat)
        {
            EvaluateHitWindow();
        }
    }

    private void Beat()
    {
        Debug.Log($"Beat");
        _isOnBeat = true;

        _beatTimer = _beatDelay;
        _hitWindowTimer = _hitWindowDelay;
    }

    private void EvaluateHitWindow()
    {
        _hitWindowTimer -= Time.deltaTime;

        if (_hitWindowTimer < 0)
        {
            _isOnBeat = false;
        }
    }

    public void DrumHit()
    {
        if (_isOnBeat || _beatTimer - _hitWindowTimer < 0)
        {
            HitOnBeat();
        }
        else
        {
            HitOffBeat();
        }
    }

    private void HitOnBeat()
    {
        Debug.Log($"Hit on beat");

        ++_beatStreak;
        _audioManager.PlayOnBeatDrumSound();
        // _feedbackManager.HitOnBeat(_beatStreak);
    }

    private void HitOffBeat()
    {
        Debug.Log($"Hit off beat");

        _beatStreak = 0;
        _audioManager.PlayOffBeatDrumSound();
        // _feedbackManager.HitOffBeat();
    }
}