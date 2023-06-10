using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _postHitWindowDelay = 0.2f; // On end side

    private NoteManager _noteManager = null;
    private Ship _ship = null;
    private DrumController _drum = null;
    private FeedbackManager _feedbackManager = null;
    private float _hitWindowTimer = 0.0f;
    private bool _isOnBeat = false;

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
    }

    public void PreBeat()
    {
        _noteManager.PreBeat();
        _isOnBeat = true;
    }

    public void Beat()
    {
        _hitWindowTimer = _postHitWindowDelay;
        _feedbackManager.ConstantBeatFeedback();
        _ship.Row();
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

    private void ResetDrums()
    {
        _drum.SetRecentlyHit(false);
    }
}