using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Tiers")]
    [SerializeField] private NoteTier _tier1NoteSets = null;
    [SerializeField] private NoteTier _tier2NoteSets = null;
    [SerializeField] private NoteTier _tier3NoteSets = null;
    private BeatTierType _currentTierType = BeatTierType.None;
    private NoteTier _currentTier = null;
    private NoteSet _currentSet = null;
    private int _currentSetCount = 0;
    private int _currentComboCount = 0;
    private bool _beatEnabled = false;

    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private HammerController _leftHammer = null;
    private HammerController _rightHammer = null;
    private bool _recentBeatSuccess = false;

    public BeatTierType CurrentTierType { get => _currentTierType; }
    public bool IsBeatEnabled { get => _beatEnabled; }

    public void ResetBeatSuccess() { _recentBeatSuccess = false; }
    public void SetBeatManager(BeatManager beatManager) { _beatManager = beatManager; }
    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }
    public void SetHammers(HammerController leftHammer, HammerController righthammer)
    {
        _leftHammer = leftHammer;
        _rightHammer = righthammer;
    }

    public NoteManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void SetupInitialNoteTier()
    {
        _currentTierType = BeatTierType.T1;
        LoadNoteTier(_currentTierType);

        _beatEnabled = true;
        _beatManager.StartBeat();
    }

    public void LoadNoteTier(BeatTierType currentTierType)
    {
        _currentTier = TranslateNoteTier(currentTierType);
        _currentSetCount = 0;
        _currentComboCount = 0;
        _currentSet = _currentTier.NoteSetList[_currentSetCount];
    }

    private NoteTier TranslateNoteTier(BeatTierType currentTierType)
    {
        switch (currentTierType)
        {
            case BeatTierType.T1:
                return _tier1NoteSets;
            case BeatTierType.T2:
                return _tier2NoteSets;
            case BeatTierType.T3:
                return _tier3NoteSets;
            default:
                Debug.LogError($"Invalid Tier Set: {currentTierType}");
                return null;
        }
    }

    public void LoadNextSet()
    {
        _currentComboCount = 0;
        ++_currentSetCount;

        if (_currentSetCount >= _currentTier.NoteSetList.Count)
        {
            if (_currentTierType == BeatTierType.T3)
            {
                _beatEnabled = false;
            }
            else
            {
                BeatTierType newTier = EvaluateNextTier();
                LoadNoteTier(newTier);
            }
        }
    }

    private BeatTierType EvaluateNextTier()
    {
        switch (_currentTierType)
        {
            case BeatTierType.T1:
                return BeatTierType.T2;
            case BeatTierType.T2:
                return BeatTierType.T3;
            default:
                Enums.InvalidSwitch(GetType(), _currentTierType.GetType());
                return BeatTierType.None;
        }
    }

    public void PreBeat()
    {
        if (_currentSet == null || _beatEnabled == false)
        {
            return;
        }

        BeatDirection nextBeat = _currentSet.NoteOrder[_currentComboCount];
        _feedbackManager.BeatBuildUpFeedback(nextBeat);
    }

    public void NoteBeat()
    {
        if (_currentSet == null || _beatEnabled == false)
        {
            return;
        }

        ++_currentComboCount;

        BeatDirection nextBeat = _currentSet.NoteOrder[_currentComboCount];
        _feedbackManager.ConstantBeatFeedback(nextBeat);

        if (_currentComboCount >= _currentSet.NoteOrder.Count)
        {
            //LoadNextSet();
        }
    }

    public void DrumHit(DrumSide drumSide, HammerSide hammerSide)
    {
        if (_beatManager.IsOnBeat || _beatManager.PreHitWindowCheck())
        {
            HitOnBeat(drumSide, hammerSide);
        }
        else
        {
            HitOffBeat(drumSide, hammerSide);
        }
    }

    private void HitOnBeat(DrumSide drumSide, HammerSide hammerSide)
    {
        if (_recentBeatSuccess == false)
        {
            _feedbackManager.OnFirstBeatFeedback(DrumSideToDirection(drumSide));
        }
        else
        {
            _feedbackManager.OnMinorBeatFeedback(DrumSideToDirection(drumSide));
        }

        PlayHammerHaptic(hammerSide, HapticIntensity.High);

        _recentBeatSuccess = true;
    }

    private void HitOffBeat(DrumSide drumSide, HammerSide hammerSide)
    {
        _feedbackManager.OffBeatFeedback(DrumSideToDirection(drumSide));
        PlayHammerHaptic(hammerSide, HapticIntensity.Low);
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

    private BeatDirection DrumSideToDirection(DrumSide hammerType)
    {
        switch (hammerType)
        {
            case DrumSide.Left:
                return BeatDirection.Left;
            case DrumSide.Right:
                return BeatDirection.Right;
            default:
                Enums.InvalidSwitch(GetType(), hammerType.GetType());
                return BeatDirection.None;
        }
    }
}
