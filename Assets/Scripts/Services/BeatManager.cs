using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _beatBuildUp = 0.75f;
    [SerializeField] private float _preHitWindowDelay = 0.2f; // On pre side
    [SerializeField] private float _postHitWindowDelay = 0.2f; // On end side
    private FeedbackManager _feedbackManager = null;
    private NoteManager _noteManager = null;
    private BeatTierType _currentTier = BeatTierType.None;
    private float _beatTimer = 0.0f;
    private float _hitWindowTimer = 0.0f;
    private bool _isOnBeat = false;
    private bool _isPlaying = false;
    private bool _beatBuildUpPlayed = false;

    public BeatTierType CurrentTier { get => _currentTier; }
    public bool IsOnBeat { get => _isOnBeat; }

    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }
    public void SetNoteManager(NoteManager noteManager) { _noteManager = noteManager; }

    public void StartBeat()
    {
        _isPlaying = true;
        _beatTimer = _beatDelay;
        _currentTier = BeatTierType.T1;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    private void Update()
    {
        if (_isPlaying == false)
        {
            return;
        }

        _beatTimer -= Time.deltaTime;

        EvaluateBeatBuildUp();

        if (_beatTimer < 0)
        {
            Beat();
        }
        else if (_isOnBeat)
        {
            EvaluateHitWindow();
        }
    }

    private void EvaluateBeatBuildUp()
    {
        if (_beatBuildUpPlayed == true)
        {
            return;
        }

        if (_beatTimer <= _beatBuildUp)
        {
            _beatBuildUpPlayed = true;
            _feedbackManager.BeatBuildUpFeedback();
        }
    }

    private void Beat()
    {
        _isOnBeat = true;
        _beatBuildUpPlayed = false;

        _feedbackManager.ConstantBeatFeedback();

        _beatTimer = _beatDelay;
        _hitWindowTimer = _postHitWindowDelay;
    }

    private void EvaluateHitWindow()
    {
        _hitWindowTimer -= Time.deltaTime;

        if (_hitWindowTimer < 0)
        {
            _isOnBeat = false;
            _noteManager.ResetBeatSuccess();
        }
    }

    public bool PreHitWindowCheck()
    {
        float delta = _beatTimer - _preHitWindowDelay;
        return delta < 0;
    }
}