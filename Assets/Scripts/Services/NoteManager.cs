using System;
using System.Collections.Generic;
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

    private LevelLoader _levelLoader = null;
    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private HammerController _leftHammer = null;
    private HammerController _rightHammer = null;
    private DrumController _rightDrum = null;
    private DrumController _leftDrum = null;
    private List<BeatDirection> _recentPlayerInput = new List<BeatDirection>();

    private Action<BeatTierType> _tierUpgrade = null;

    public BeatTierType CurrentTierType { get => _currentTierType; }
    public bool IsBeatEnabled { get => _beatEnabled; }

    public void SubscribeTierUpgrade(Action<BeatTierType> action) { _tierUpgrade += action; }
    public void UnsubscribeTierUpgrade(Action<BeatTierType> action) { _tierUpgrade -= action; }
    public void SetBeatManager(BeatManager beatManager) { _beatManager = beatManager; }
    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }

    public void SetHammers(HammerController leftHammer, HammerController righthammer)
    {
        _leftHammer = leftHammer;
        _rightHammer = righthammer;
    }

    public void SetDrums(DrumController rightDrum, DrumController leftDrum)
    {
        _rightDrum = rightDrum;
        _leftDrum = leftDrum;
    }

    public NoteManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        _levelLoader = ServiceLocator.Get<LevelLoader>();

        return this;
    }

    public void SetupInitialNoteTier()
    {
        _currentTierType = BeatTierType.T1;
        LoadTier(_currentTierType);

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

        _recentPlayerInput.Clear();
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
            _beatEnabled = false;

            if (_currentTierType == BeatTierType.T3)
            {
                Debug.Log($"Beat Tiers Cleared");
                _levelLoader.WrapUpSequence();
            }
            else
            {

                BeatTierType newTier = EvaluateNextTier();
                LoadTier(newTier);
            }
        }
    }

    private void LoadTier(BeatTierType currentTierType)
    {
        Debug.Log($"Loading New Tier [{currentTierType}]");

        _currentTierType = currentTierType;
        _currentTier = TranslateNoteTier(currentTierType);

        _currentComboSet = 0;
        _currentComboCount = 0;

        _currentCombo = _currentTier.NoteCombos[_currentComboSet];

        _tierUpgrade?.Invoke(_currentTierType);

        _beatEnabled = true;
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
        BeatDirection beatDirection = DrumSideToDirection(drumSide);
        if (beatDirection == BeatDirection.Left)
        {
            HitDrumOnBeat(beatDirection, _leftDrum, hammerSide);
        }
        else
        {
            HitDrumOnBeat(beatDirection, _rightDrum, hammerSide);
        }

        _recentPlayerInput.Add(beatDirection);
    }

    private void HitDrumOnBeat(BeatDirection beatDirection, DrumController drum, HammerSide hammerSide)
    {
        if (drum.RecentlyHit == false)
        {
            _feedbackManager.OnFirstBeatFeedback(beatDirection);
        }
        else
        {
            _feedbackManager.OnMinorBeatFeedback(beatDirection);
        }

        PlayHammerHaptic(hammerSide, HapticIntensity.High);

        drum.SetRecentlyHit(true);
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
