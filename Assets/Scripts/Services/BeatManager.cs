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
        _isOnBeat = true;
    }

    public void Beat()
    {
        _hitWindowTimer = _postHitWindowDelay;

        if (_constantBeatonNextBeat)
        {
            ActivateConstantBeat();
        }
    }

    private void ActivateConstantBeat()
    {
        _feedbackManager.ConstantBeatFeedback();
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
            ResetDrums();
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

    private void ResetDrums()
    {
        _drum.SetRecentlyHit(false);
    }
}