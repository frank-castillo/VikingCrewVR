using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Tiers")]
    [SerializeField] private NoteSetTier _tier1NoteSets = null;
    [SerializeField] private NoteSetTier _tier2NoteSets = null;
    [SerializeField] private NoteSetTier _tier3NoteSets = null;
    private NoteSetTier _currentSetTier = null;
    private NoteSet _currentSet = null;
    private int _currentTierCount = 0;
    private int _currentSetCount = 0;
    private int _comboCount = 0;

    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private HammerController _leftHammer = null;
    private HammerController _rightHammer = null;
    private bool _recentBeatSuccess = false;

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

        _currentTierCount = 1;

        return this;
    }

    public void LoadNextNoteTier()
    {
        if (_currentTierCount == 1)
        {
            _currentSetTier = _tier1NoteSets;
        }
        else if (_currentTierCount == 2)
        {
            _currentSetTier = _tier2NoteSets;
        }
        else if (_currentTierCount == 3)
        {
            _currentSetTier = _tier3NoteSets;
        }

        _currentSetCount = 0;
        _comboCount = 0;
        _currentSet = _currentSetTier.NoteSetList[_currentSetCount];
    }

    public void LoadNextCombo()
    {
        _comboCount = 0;
        ++_currentSetCount;

        if (_currentSetCount >= _currentTierCount)
        {
            ++_currentTierCount;
            LoadNextNoteTier();
        }
    }

    public void NoteBeat()
    {
        if (_currentSet == null)
        {
            return;
        }

        ++_comboCount;

        _feedbackManager.ConstantBeatFeedback(BeatDirection.Both);

        if (_comboCount >= _currentSet.NoteOrder.Count)
        {
            LoadNextCombo();
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
