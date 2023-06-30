using System;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Tiers")]
    [SerializeField] private NoteTier _tier1NoteCombos = null;
    [SerializeField] private NoteTier _tier2NoteCombos = null;
    [SerializeField] private NoteTier _tier3NoteComobs = null;
    [SerializeField] private float _tierDelay = 2.0f;
    private float _tierTimer = 0.0f;

    // Core Refrences
    private AudioManager _audioManager = null;
    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private LevelLoader _levelLoader = null;
    private ProgressEvaluation _progressionEvaluation = null;
    private HammerController _leftHammer = null;
    private HammerController _rightHammer = null;
    private DrumController _drum = null;

    // Note Emmiter
    private TierType _currentTierType = TierType.None;
    private NoteTier _currentTier = null;
    private Notes _nextNote = null;
    private int _noteProgress = 0;
    private float _emitterDelay = 0.0f;

    // Pause Bools
    private bool _loadingTierPause = false;
    private bool _wrapUpActive = false;
    private bool _emitterActive = false;

    private Action<TierType> _tierUpgrade = null;

    public TierType CurrentTierType { get => _currentTierType; }
    public bool WrapUpActive { get => _wrapUpActive; }

    public void SubscribeTierUpgrade(Action<TierType> action) { _tierUpgrade += action; }
    public void UnsubscribeTierUpgrade(Action<TierType> action) { _tierUpgrade -= action; }

    public void SetBeatManager(BeatManager beatManager) { _beatManager = beatManager; }
    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }
    public void SetShip(Ship ship) { _drum = ship.Drum; }

    public void SetHammers(HammerController leftHammer, HammerController righthammer)
    {
        _leftHammer = leftHammer;
        _rightHammer = righthammer;
    }

    public NoteManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        _levelLoader = ServiceLocator.Get<LevelLoader>();
        _audioManager = ServiceLocator.Get<AudioManager>();

        _progressionEvaluation = GetComponent<ProgressEvaluation>();

        return this;
    }

    private void Update()
    {
        if (_wrapUpActive || _emitterActive == false)
        {
            return;
        }

        if (_loadingTierPause)
        {
            EvaluateTierTimer();

            return;
        }

        _emitterDelay -= Time.deltaTime;

        if (_emitterDelay < 0.0f)
        {
            EmitNextParticle();
        }
    }

    private void EvaluateTierTimer()
    {
        _tierTimer -= Time.deltaTime;
        if (_tierTimer < 0.0f)
        {
            _loadingTierPause = false;
            Debug.Log($"Finished Loading");
        }
    }

    public void SetupInitialNoteTier()
    {
        _currentTierType = TierType.T1;
        LoadTier(_currentTierType, false);

        _emitterActive = true;
    }

    private NoteTier TranslateNoteTier(TierType currentTierType)
    {
        switch (currentTierType)
        {
            case TierType.T1:
                return _tier1NoteCombos;
            case TierType.T2:
                return _tier2NoteCombos;
            case TierType.T3:
                return _tier3NoteComobs;
            default:
                Debug.LogError($"Invalid Tier Set: {currentTierType}");
                return null;
        }
    }

    private TierType EvaluateNextTier()
    {
        if (_progressionEvaluation.MoveToNextTier() == false)
        {
            return _currentTierType;
        }

        switch (_currentTierType)
        {
            case TierType.T1:
                return TierType.T2;
            case TierType.T2:
                return TierType.T3;
            default:
                Enums.InvalidSwitch(GetType(), _currentTierType.GetType());
                return TierType.None;
        }
    }

    private void EmitNextParticle()
    {
        _drum.EmitParticle();
        LoadNextBeat();
    }

    private void LoadNextBeat()
    {
        ++_noteProgress;
        if (_noteProgress >= _currentTier.NoteList.Count)
        {
            LoadNextTier();
        }
        else
        {
            LoadNextNote();
        }
    }

    private void LoadNextTier()
    {
        if (_currentTierType == TierType.T3)
        {
            Debug.Log($"Beat Tiers Cleared");
            _levelLoader.WrapUpSequence();
            _wrapUpActive = true;
            _emitterActive = false;
        }
        else
        {
            TierType newTier = EvaluateNextTier();
            LoadTier(newTier, true);
        }
    }

    private void LoadTier(TierType currentTierType, bool tierPause)
    {
        Debug.Log($"Loading New Tier [{currentTierType}]");

        _currentTierType = currentTierType;
        _currentTier = TranslateNoteTier(currentTierType);

        _noteProgress = 0;
        LoadNextNote();

        _progressionEvaluation.Prepare(_currentTierType, _currentTier.NoteList.Count);

        _tierUpgrade?.Invoke(_currentTierType);
        _audioManager.PlayMusic(_currentTierType);

        if (tierPause)
        {
            _tierTimer = _tierDelay;
            _loadingTierPause = true;
        }
    }

    private void LoadNextNote()
    {
        _nextNote = _currentTier.NoteList[_noteProgress];
        _emitterDelay = _nextNote.SpawnDelay;
    }

    public void DrumHit(HammerSide hammerSide)
    {
        if (_beatManager.IsOnBeat)
        {
            HitOnBeat(hammerSide);
        }
        else
        {
            HitOffBeat(hammerSide);
        }
    }

    private void HitOnBeat(HammerSide hammerSide)
    {
        HitDrumOnBeat(_drum, hammerSide);
    }

    private void HitDrumOnBeat(DrumController drum, HammerSide hammerSide)
    {
        if (drum.RecentlyHit == false)
        {
            _feedbackManager.OnFirstBeatFeedback();
            _progressionEvaluation.Success();
        }
        else
        {
            _feedbackManager.OnMinorBeatFeedback();
        }

        PlayHammerHaptic(hammerSide, HapticIntensity.High);

        drum.SetRecentlyHit(true);
    }

    private void HitOffBeat(HammerSide hammerSide)
    {
        _feedbackManager.OffBeatFeedback();
        PlayHammerHaptic(hammerSide, HapticIntensity.Low);
        ProgressionFail();
    }

    public void ProgressionFail()
    {
        _progressionEvaluation.Fail();
    }

    private void PlayHammerHaptic(HammerSide hammerSide, HapticIntensity hapticIntensity)
    {
        switch (hammerSide)
        {
            case HammerSide.Left:
                _leftHammer.PlayHaptic(hapticIntensity);
                break;
            case HammerSide.Right:
                _rightHammer.PlayHaptic(hapticIntensity);
                break;
            default:
                Enums.InvalidSwitch(GetType(), hammerSide.GetType());
                break;
        }
    }
}
