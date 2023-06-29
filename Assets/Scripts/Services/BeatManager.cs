using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _rowDelay = 2.0f;
    private float _rowTimer = 0.0f;

    private NoteManager _noteManager = null;
    private Ship _ship = null;
    private DrumController _drum = null;
    private FeedbackManager _feedbackManager = null;
    private bool _isOnBeat = false;
    private bool _rowOnNextBeat = false;
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
            _rowTimer -= Time.deltaTime;

            if (_rowTimer < 0)
            {
                _rowTimer = _rowDelay;
                _ship.Row();
            }
            return;
        }
        else
        {
            if (_rowOnNextBeat == false)
            {
                EvaluateConstantBeat();
            }
        }
    }

    public void PreBeat()
    {
        if (_rowOnNextBeat)
        {
            _ship.Row();

            _rowTimer = _rowDelay;
            _rowOnNextBeat = false;
        }
    }

    public void ActivateOnBeat()
    {
        _drum.SetRecentlyHit(false);
        _isOnBeat = true;
        _feedbackManager.BeatBuildUpFeedback();
    }

    public void EndOnBeat()
    {
        _isOnBeat = false;
        if (_drum.RecentlyHit == false)
        {
            InactiveFail();
        }
    }

    private void InactiveFail()
    {
        _drum.PlayFailureVFX();
        _noteManager.ProgressionFail();
    }

    public void Beat()
    {
        _feedbackManager.ConstantBeatFeedback();
    }

    private void EvaluateConstantBeat()
    {
        _rowTimer -= Time.deltaTime;
        if (_rowTimer < 0)
        {
            _rowOnNextBeat = true;
        }
    }
}