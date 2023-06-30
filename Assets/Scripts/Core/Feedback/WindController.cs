using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindController : MonoBehaviour
{
    [Header("Emission")]
    [SerializeField] private int _emmisionRateT1 = 2;
    [SerializeField] private int _emmisionRateT2 = 5;
    [SerializeField] private int _emmisionRateT3 = 8;

    [Header("References")]
    [SerializeField] private ParticleSystem _leftWind = null;
    [SerializeField] private ParticleSystem _rightWind = null;
    EmissionModule _leftWindEmission;
    EmissionModule _rightWindEmission;

    private NoteManager _noteManager = null;
    private bool _initialized = false;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();

        _leftWindEmission = _leftWind.emission;
        _rightWindEmission = _rightWind.emission;

        SetupEvents();

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
        _noteManager.SubscribeTierUpgrade(TierEvaluation);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTierUpgrade(TierEvaluation);
    }

    private void TierEvaluation(TierType beatTierType)
    {
        switch (beatTierType)
        {
            case TierType.T1:
                LevelUp(1);
                break;
            case TierType.T2:
                LevelUp(2);
                break;
            case TierType.T3:
                LevelUp(3);
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
                break;
        }
    }

    private void LevelUp(int newLevel)
    {
        if (newLevel == 1)
        {
            _leftWindEmission.rateOverTime = _emmisionRateT1;
            _rightWindEmission.rateOverTime = _emmisionRateT1;
        }
        else if (newLevel == 2)
        {
            _leftWindEmission.rateOverTime = _emmisionRateT2;
            _rightWindEmission.rateOverTime = _emmisionRateT2;
        }
        else if (newLevel == 3)
        {
            _leftWindEmission.rateOverTime = _emmisionRateT3;
            _rightWindEmission.rateOverTime = _emmisionRateT3;
        }
    }
}