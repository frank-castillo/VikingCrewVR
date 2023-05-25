using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _closeFog = null;
    [SerializeField] private AnimationCurve _animationCurve = new AnimationCurve();
    [SerializeField] private int _changeAmount = 1;
    private FeedbackManager _feedbackManager = null;
    private BeatManager _beatManager = null;

    public void Initialize()
    {
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _beatManager = ServiceLocator.Get<BeatManager>();

        Subscriptions();
    }

    private void Subscriptions()
    {
        _feedbackManager.OnBeatHitSubscribe(LowerFog);
        _feedbackManager.RepeatedMissSubscribe(IncreaseFog);
    }

    private void LowerFog()
    {
        var emission = _closeFog.emission;
        emission.rateOverTime = 50.0f;
    }

    private void IncreaseFog()
    {

    }
}
