using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _postHitWindowDelay = 0.2f; // On end side
    [SerializeField] private float _constantBeatDelay = 2.0f;
    private float _hitWindowTimer = 0.0f;
    private float _constantBeatTimer = 0.0f;

    private NoteManager _noteManager = null;
    private Ship _ship = null;
    private DrumController _drum = null;
    private FeedbackManager _feedbackManager = null;
    private bool _isOnBeat = false;
    private bool _constantBeatonNextBeat = false;

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

        return this;
    }

    private void Update()
    {
        if (_isOnBeat)
        {
            EvaluateHitWindow();
        }

        if (_constantBeatonNextBeat == false)
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
        _hitWindowTimer = _postHitWindowDelay;
    }

    public void Beat()
    {
        _feedbackManager.ConstantBeatFeedback();

        if (_constantBeatonNextBeat)
        {
            ActivateConstantBeat();
        }
    }

    private void ActivateConstantBeat()
    {
        _ship.Row();

        _constantBeatTimer = _constantBeatDelay;
        _constantBeatonNextBeat = false;
    }

    private void EvaluateHitWindow()
    {
        _hitWindowTimer -= Time.deltaTime;
        if (_hitWindowTimer < 0)
        {
            _isOnBeat = false;
        }
    }

    private void EvaluateConstantBeat()
    {
        _constantBeatTimer -= Time.deltaTime;
        if (_constantBeatTimer < 0)
        {
            _constantBeatonNextBeat = true;
        }
    }
}