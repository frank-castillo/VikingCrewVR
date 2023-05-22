using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesProgressionController : MonoBehaviour
{
    [SerializeField] private GameObject _runeMeshTier1 = null;
    [SerializeField] private GameObject _runeMeshTier2 = null;
    [SerializeField] private GameObject _runeMeshTier3 = null;

    [SerializeField] private ParticleSystem _runeParticlesTier1 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier2 = null;
    [SerializeField] private ParticleSystem _runeParticlesTier3 = null;

    [SerializeField] private ParticleSystem _borderParticles = null;

    private int _currentLevel = 0;
    private FeedbackManager _feedbackManager = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        LevelUp(1);

        _runeMeshTier1.SetActive(true);
        _runeMeshTier2.SetActive(true);
        _runeMeshTier3.SetActive(true);

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        FeedbackSubscriptions();
    }

    private void FeedbackSubscriptions()
    {
        _feedbackManager.ConstantBeatSubscribe(OnBeatPulse);
    }

    private void OnDestroy()
    {
        _feedbackManager.ConstantBeatUnsubscribe(OnBeatPulse);
    }


    public void LevelUp(int newLevel)
    {
        if (_currentLevel == newLevel)
        {
            return;
        }

        _currentLevel = newLevel;

        //if (newLevel == 1)
        //{
       
        //    _runeParticlesTier2.gameObject.SetActive(false);
        //    _runeParticlesTier3.gameObject.SetActive(false);
        //}
        //else if (newLevel == 2)
        //{
        //    _runeParticlesTier2.gameObject.SetActive(true);
        //}
        //else if (newLevel == 3)
        //{
        //    _runeParticlesTier3.gameObject.SetActive(true);
        //}
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
