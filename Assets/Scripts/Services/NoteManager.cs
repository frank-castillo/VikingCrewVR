using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Tiers")]
    [SerializeField] private NoteTier _tier1NoteCombos = null;
    [SerializeField] private NoteTier _tier2NoteCombos = null;
    [SerializeField] private NoteTier _tier3NoteComobs = null;

    [Header("Delays")]
    [SerializeField] private float _comboChangeDelay = 2.0f;
    [SerializeField] private float _fadeDuration = 2.0f;
    [SerializeField] private float _postFadeToBlackDelay = 2.0f;
    [SerializeField] private float _returnToCLearDelay = 2.0f;

    private BeatTierType _currentTierType = BeatTierType.None;
    private NoteTier _currentTier = null;
    private NoteCombo _currentCombo = null;
    private int _currentComboSet = 0;
    private int _currentComboCount = 0;

    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private LevelLoader _levelLoader = null;
    private Ship _ship = null;
    private HammerController _leftHammer = null;
    private HammerController _rightHammer = null;
    private DrumController _drum = null;

    private Action<BeatTierType> _tierUpgrade = null;

    public BeatTierType CurrentTierType { get => _currentTierType; }


    public void SubscribeTierUpgrade(Action<BeatTierType> action) { _tierUpgrade += action; }
    public void UnsubscribeTierUpgrade(Action<BeatTierType> action) { _tierUpgrade -= action; }

    public void SetBeatManager(BeatManager beatManager) { _beatManager = beatManager; }
    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }
    public void SetShip(Ship ship)
    {
        _ship = ship;
        _drum = ship.Drum;
    }

    public void SetHammers(HammerController leftHammer, HammerController righthammer)
    {
        _leftHammer = leftHammer;
        _rightHammer = righthammer;
    }

    public void SetDrums(DrumController drum)
    {
        _drum = drum;
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
        _feedbackManager.BeatBuildUpFeedback(BeatDirection.Both);
    }

    public void NoteBeat()
    {
        _feedbackManager.ConstantBeatFeedback(BeatDirection.Both);
        _ship.Row();
    }

    public void EndOfBeat()
    {
        LoadNextBeat();
    }

    private void LoadNextBeat()
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
        ++_currentComboSet;

        if (_currentComboSet >= _currentTier.NoteCombos.Count)
        {
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
        else
        {
            _currentCombo = _currentTier.NoteCombos[_currentComboSet];
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
    }

    public void DrumHit(HammerSide hammerSide)
    {
        if (_beatManager.IsOnBeat || _beatManager.PreHitWindowCheck())
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
        BeatDirection beatDirection = BeatDirection.Both;
        HitDrumOnBeat(beatDirection, _drum, hammerSide);
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

    private void HitOffBeat(HammerSide hammerSide)
    {
        _feedbackManager.OffBeatFeedback(BeatDirection.Both);
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
}
