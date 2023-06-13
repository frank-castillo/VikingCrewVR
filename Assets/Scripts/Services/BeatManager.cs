using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _constantBeatDelay = 2.0f;
    private float _hitWindowTimer = 0.0f;
    private float _constantBeatTimer = 0.0f;

    private NoteManager _noteManager = null;
    private Ship _ship = null;
    private DrumController _drum = null;
    private FeedbackManager _feedbackManager = null;
    private bool _isOnBeat = false;
    private bool _constantBeatOnNextBeat = false;
    private bool _initialized = true;

    public bool IsOnBeat { get => _isOnBeat; }

    public void SetNoteManager(NoteManager noteManager) { _noteManager = noteManager; }
    public void SetShip(Ship ship)
    {
        _ship = ship;
        _drum = ship.Drum;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _initialized = true;

        return this;
    }

    private void Update()
    {
        if (_initialized == false)
        {
            return;
        }

        if (_noteManager.WrapUpActive)
        {
            _constantBeatTimer -= Time.deltaTime;

            if (_constantBeatTimer < 0)
            {
                _constantBeatTimer = _constantBeatDelay;
                _ship.Row();
            }
            return;
        }

        if (_constantBeatOnNextBeat == false)
        {
            EvaluateConstantBeat();
        }
    }

    public void PreBeat()
    {
        _noteManager.PreBeat();
    }

    public void ActivateOnBeat()
    {
        _drum.SetRecentlyHit(false);
        _isOnBeat = true;
    }

    public void EndOnBeat()
    {
        _isOnBeat = false;
        if (_drum.RecentlyHit == false)
        {
            _drum.PlayFailureVFX();
        }
    }

    public void Beat()
    {
        _feedbackManager.ConstantBeatFeedback();

        if (_constantBeatOnNextBeat)
        {
            ActivateConstantBeat();
        }
    }

    private void ActivateConstantBeat()
    {
        _ship.Row();

        _constantBeatTimer = _constantBeatDelay;
        _constantBeatOnNextBeat = false;
    }

    private void EvaluateConstantBeat()
    {
        _constantBeatTimer -= Time.deltaTime;
        if (_constantBeatTimer < 0)
        {
            _constantBeatOnNextBeat = true;
        }
    }
}