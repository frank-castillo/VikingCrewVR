using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Tiers")]
    [SerializeField] private NoteTier _tier1NoteCombos = null;
    [SerializeField] private NoteTier _tier2NoteCombos = null;
    [SerializeField] private NoteTier _tier3NoteComobs = null;
    private BeatTierType _currentTierType = BeatTierType.None;
    private NoteTier _currentTier = null;
    private NoteCombo _currentCombo = null;
    private int _currentComboSet = 0;
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

    private NoteTier TranslateNoteTier(BeatTierType currentTierType)
    {
        switch (currentTierType)
        {
            case BeatTierType.T1:
                return _tier1NoteCombos;
            case BeatTierType.T2:
                return _tier2NoteCombos;
            case BeatTierType.T3:
                return _tier3NoteComobs;
            default:
                Debug.LogError($"Invalid Tier Set: {currentTierType}");
                return null;
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
        if (_currentCombo == null || _beatEnabled == false)
        {
            return;
        }

        BeatDirection nextBeat = _currentCombo.ComboList[_currentComboCount];
        _feedbackManager.BeatBuildUpFeedback(nextBeat);
    }

    public void NoteBeat()
    {
        if (_currentCombo == null || _beatEnabled == false)
        {
            return;
        }

        BeatDirection nextBeat = _currentCombo.ComboList[_currentComboCount];
        _feedbackManager.ConstantBeatFeedback(nextBeat);
    }

    public void LoadNextBeat()
    {
        ++_currentComboCount;
        if (_currentComboCount >= _currentCombo.ComboList.Count)
        {
            LoadNextSet();
        }
    }

    private void LoadNextSet()
    {
        _currentComboCount = 0;
        _currentCombo = _currentTier.NoteCombos[_currentComboSet];
        ++_currentComboSet;

        if (_currentComboSet >= _currentTier.NoteCombos.Count)
        {

            if (_currentTierType == BeatTierType.T3)
            {
                Debug.Log($"Beat Tiers Cleared");
                _beatEnabled = false;
            }
            else
            {
                Debug.Log($"Loading New Tier [{_currentTier}]");

                BeatTierType newTier = EvaluateNextTier();
                LoadNoteTier(newTier);
            }
        }
    }

    private void LoadNoteTier(BeatTierType currentTierType)
    {
        _currentTierType = currentTierType;
        _currentTier = TranslateNoteTier(currentTierType);

        _currentComboSet = 0;
        _currentComboCount = 0;

        _currentCombo = _currentTier.NoteCombos[_currentComboSet];
    }

    public void DrumHit(DrumSide drumSide, HammerSide hammerSide)
    {
        BeatDirection nextBeat = _currentCombo.ComboList[_currentComboCount];
        if (IsMatchingSideOrBoth(nextBeat, drumSide))
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

    private bool IsMatchingSideOrBoth(BeatDirection beatDirection, DrumSide drumSide)
    {
        if (beatDirection == BeatDirection.Both)
        {
            return true;
        }

        if (beatDirection == BeatDirection.Left && drumSide == DrumSide.Left)
        {
            return true;
        }

        if (beatDirection == BeatDirection.Right && drumSide == DrumSide.Right)
        {
            return true;
        }

        return false;
    }
}
