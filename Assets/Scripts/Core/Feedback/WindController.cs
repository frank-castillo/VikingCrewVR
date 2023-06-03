using UnityEngine;
using static UnityEngine.ParticleSystem;

public class WindController : MonoBehaviour
{
    [SerializeField] private int _emmisionRateT1 = 2;
    [SerializeField] private int _emmisionRateT2 = 5;
    [SerializeField] private int _emmisionRateT3 = 8;

    [SerializeField] private ParticleSystem _leftWind = null;
    EmissionModule _leftWindEmission;

    [SerializeField] private ParticleSystem _rightWind = null;
    EmissionModule _rightWindEmission;

    private NoteManager _noteManager = null;
    private FeedbackManager _feedbackManager = null;

    private int _currentTier = 0;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _leftWindEmission = _leftWind.emission;
        _rightWindEmission = _rightWind.emission;
        _currentTier = 1;

        _feedbackManager.OnBeatFirstHitSubscribe(TierEvaluation);
    }

    private void OnDestroy()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(TierEvaluation);
    }

    private void TierEvaluation(BeatDirection beatDirection)
    {
        switch (_noteManager.CurrentTierType)
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
                Enums.InvalidSwitch(GetType(), _noteManager.CurrentTierType.GetType());
                break;
        }
    }

    private void LevelUp(int newLevel)
    {
        if (_currentTier == newLevel)
        {
            return;
        }

        _currentTier = newLevel;

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