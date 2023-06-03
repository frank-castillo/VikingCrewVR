using UnityEngine;

public class FogController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _closeFog = null;
    [SerializeField] private float _tier1FogEmmision = 35.0f;
    [SerializeField] private float _tier2FogEmmision = 20.0f;
    [SerializeField] private float _tier3FogEmmision = 10.0f;
    private ParticleSystem.EmissionModule _emission = default;

    private NoteManager _noteManager = null;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();

        _emission = _closeFog.emission;

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SubscribeEvents()
    {
        _noteManager.SubscribeTier2Upgrade(ChangeFog);
        _noteManager.SubscribeTier3Upgrade(ChangeFog);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTier2Upgrade(ChangeFog);
        _noteManager.UnsubscribeTier3Upgrade(ChangeFog);
    }

    public void ChangeFog()
    {
        switch (_noteManager.CurrentTierType)
        {
            case BeatTierType.T1:
                _emission.rateOverTime = _tier1FogEmmision;
                break;
            case BeatTierType.T2:
                _emission.rateOverTime = _tier1FogEmmision;
                break;
            case BeatTierType.T3:
                _emission.rateOverTime = _tier1FogEmmision;
                break;
            default:
                Enums.InvalidSwitch(GetType(), _noteManager.CurrentTierType.GetType());
                break;
        }
    }
}
