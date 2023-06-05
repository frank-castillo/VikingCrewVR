﻿using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _closeFog = null;
    [SerializeField] private float _tier1FogEmmision = 35.0f;
    [SerializeField] private float _tier2FogEmmision = 20.0f;
    [SerializeField] private float _tier3FogEmmision = 10.0f;
    private ParticleSystem.EmissionModule _emission = default;
    private NoteManager _noteManager = null;
    private bool _initialized = false;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();

        _emission = _closeFog.emission;

        SetupEvents();

        ChangeFog(BeatTierType.T1);

        _initialized = true;
    }

    private void OnDestroy()
    {
        if (_initialized == false)
        {
            return;
        }

        UnsubscribeEvents();
    }

    private void SetupEvents()
    {
        _noteManager.SubscribeTierUpgrade(ChangeFog);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTierUpgrade(ChangeFog);
    }

    public void ChangeFog(BeatTierType beatTierType)
    {
        switch (beatTierType)
        {
            case BeatTierType.T1:
                _emission.rateOverTime = _tier1FogEmmision;
                break;
            case BeatTierType.T2:
                _emission.rateOverTime = _tier2FogEmmision;
                break;
            case BeatTierType.T3:
                _emission.rateOverTime = _tier3FogEmmision;
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
                break;
        }
    }
}
