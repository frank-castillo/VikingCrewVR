using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _hitWindow = 0.25f;

    [Header("References")]
    [SerializeField] private FeedbackManager _feedbackManager = null;

    private float _timer = 0.0f;
    private bool _isOnBeat = false;
    private bool _isPlaying = false;

    public void StopBeat() { _isPlaying = false; }
    public void StartBeat()
    {
        _isPlaying = true;
        _timer = _beatDelay;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager.Initialize();

        return this;
    }

    private void Update()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            OnBeatSwap();
        }
    }

    private void OnBeatSwap()
    {
        _isOnBeat = !_isOnBeat;
        if (_isOnBeat)
        {
            Debug.Log($"Start Beat: Hit now!");
            _timer = _hitWindow;
        }
        else
        {
            Debug.Log($"End Beat: Stop hitting drums");
            _timer = _beatDelay;
        }
    }

    public void Hit()
    {
        if (_isOnBeat)
        {

        }
        else
        {

        }
    }
}