using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _closeFog = null;
    [SerializeField] private float _minFogEmmision = 0.0f;
    [SerializeField] private float _maxFogEmmision = 35.0f;
    [SerializeField] private float _rateChange = 1.0f;
    [SerializeField] private int _decayMultiplier = 3;
    private ParticleSystem.EmissionModule _emission = default;

    private FeedbackManager _feedbackManager = null;

    public void Initialize()
    {
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

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
        _feedbackManager.OnBeatFirstHitSubscribe(LowerFog);
        _feedbackManager.OffBeatMissSubscribe(LowerFog);
        _feedbackManager.RepeatedMissSubscribe(IncreaseFog);
    }

    private void UnsubscribeEvents()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(LowerFog);
        _feedbackManager.OffBeatMissUnsubscribe(LowerFog);
        _feedbackManager.RepeatedMissUnsubscribe(IncreaseFog);
    }

    private void LowerFog(BeatDirection beatDirection)
    {
        if (_emission.rateOverTime.constant > _minFogEmmision)
        {
            float newRate = _emission.rateOverTime.constant - _rateChange;
            _emission.rateOverTime = newRate;
        }
    }

    private void IncreaseFog(BeatDirection beatDirection)
    {
        if (_emission.rateOverTime.constant < _maxFogEmmision)
        {
            float newRate = _emission.rateOverTime.constant + _rateChange * _decayMultiplier;
            _emission.rateOverTime = newRate;
        }
    }
}
