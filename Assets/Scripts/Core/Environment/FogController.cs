using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _closeFog = null;
    [SerializeField] private float _minFogEmmision = 0.0f;
    [SerializeField] private float _maxFogEmmision = 35.0f;
    [SerializeField] private float _rateChange = 1.0f;
    private ParticleSystem.EmissionModule _emission = default;

    private FeedbackManager _feedbackManager = null;
    private BeatManager _beatManager = null;

    public void Initialize()
    {
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _beatManager = ServiceLocator.Get<BeatManager>();

        _emission = _closeFog.emission;
        _emission.rateOverTime = _maxFogEmmision;

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _feedbackManager.OnBeatHitSubscribe(LowerFog);
        _feedbackManager.OffBeatMissSubscribe(LowerFog);
        _feedbackManager.RepeatedMissSubscribe(IncreaseFog);
    }

    private void UnsubscribeEvents()
    {
        _feedbackManager.OnBeatHitUnsubscribe(LowerFog);
        _feedbackManager.OffBeatMissUnsubscribe(LowerFog);
        _feedbackManager.RepeatedMissUnsubscribe(IncreaseFog);
    }

    private void LowerFog()
    {
        if (_emission.rateOverTime.constant > _minFogEmmision)
        {
            float newRate = _emission.rateOverTime.constant - _rateChange;
            _emission.rateOverTime = newRate;
        }
    }

    private void IncreaseFog()
    {
        if (_emission.rateOverTime.constant < _maxFogEmmision)
        {
            float newRate = _emission.rateOverTime.constant + _rateChange * 3;
            _emission.rateOverTime = newRate;
        }
    }
}
