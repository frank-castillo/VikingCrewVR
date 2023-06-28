using UnityEngine;

public class RunesProgressionController : MonoBehaviour
{
    [SerializeField] private ParticleSystem _runeParticlesTier1 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier2 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier3 = null;
    [SerializeField] private ParticleSystem _borderParticles = null;

    private FeedbackManager _feedbackManager = null;
    private NoteManager _noteManager = null;
    private int _currentLevel = 0;
    private bool _initialized = false;

    public void Initialize()
    {
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _noteManager = ServiceLocator.Get<NoteManager>();

        SetupEvents();
        LevelUp(1);

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
        _feedbackManager.SubscribeBeatBuildUp(OnBeatPulse);
        _noteManager.SubscribeTierUpgrade(CheckLevelUp);
    }

    private void UnsubscribeEvents()
    {
        _feedbackManager.UnsubscribeBeatBuildUp(OnBeatPulse);
        _noteManager.UnsubscribeTierUpgrade(CheckLevelUp);
    }

    private void CheckLevelUp(BeatTierType beatTierType)
    {
        switch (beatTierType)
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
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
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
