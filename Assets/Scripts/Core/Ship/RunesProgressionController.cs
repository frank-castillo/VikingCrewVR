using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesProgressionController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _runeParticlesTier1 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier2 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier3 = null;

    [SerializeField] private ParticleSystem _borderParticles = null;

    private int _currentLevel = 0;
    private FeedbackManager _feedbackManager = null;
    private BeatManager _beatManager = null;

    public void Initialize()
    {
        LevelUp(1);
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _beatManager = ServiceLocator.Get<BeatManager>();
        FeedbackSubscriptions();
    }

    private void FeedbackSubscriptions()
    {
        _feedbackManager.ConstantBeatSubscribe(OnBeatPulse);
        _feedbackManager.OnBeatHitSubscribe(CheckLevelUp);
    }

    private void OnDestroy()
    {
        _feedbackManager.ConstantBeatUnsubscribe(OnBeatPulse);
        _feedbackManager.OnBeatHitUnsubscribe(CheckLevelUp);
    }

    private void CheckLevelUp()
    {
        switch (_beatManager.CurrentTier)
        {
            case BeatTierType.T1:
                LevelUp(1);
                break;
            case BeatTierType.T2:
                LevelUp(2);
                break;
            case BeatTierType.T3:
                LevelUp(3);
                break;
            default:
                Enums.InvalidSwitch(GetType(), _beatManager.CurrentTier.GetType());
                break;
        }
    }

    public void LevelUp(int newLevel)
    {
        if (_currentLevel == newLevel)
        {
            return;
        }

        _currentLevel = newLevel;
    }

    private void OnBeatPulse()
    {
        _borderParticles.Play();

        if (_currentLevel == 1)
        {
            _runeParticlesTier1.Play();
        }
        else if (_currentLevel == 2)
        {
            _runeParticlesTier1.Play();
            _runeParticlesTier2.Play();
        }
        else if (_currentLevel == 3)
        {
            _runeParticlesTier1.Play();
            _runeParticlesTier2.Play();
            _runeParticlesTier3.Play();
        }
    }
}
