using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionTreadmill : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 100f;
    [SerializeField] private float _minSpeed = 50f;
    [SerializeField] private float _speedModifier = 0.3f;
    [SerializeField] private Transform _limitOfMap = null;
    [SerializeField] private Transform _startOfMap = null;
    [SerializeField] private Transform _regionFolder = null;
    [SerializeField] private Transform _finalRegion = null;

    private FeedbackManager _feedbackManager = null;
    private List<Region> _regions = new List<Region>();

    private float _speed = 5.0f;
    private float _lerpDuration = 1.0f;
    private bool _movementEnabled = false;
    private bool _inTransition = false;
    private bool _isEnding = false;
    private bool _finalIslandReadyToMove = false;

    public void EnableMovement(bool enable) { _movementEnabled = enable; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        foreach (Transform child in _regionFolder)
        {
            var region = child.GetComponent<Region>();
            if(region.gameObject.activeInHierarchy)
            {
                region.Initialize();
                _regions.Add(region);
            }
        }

        _speed = _minSpeed;

        _feedbackManager.OnBeatFirstHitSubscribe(IncreaseSpeed);
        _feedbackManager.OffBeatMissSubscribe(DecreaseSpeed);
        _feedbackManager.RepeatedMissSubscribe(DecreaseSpeed);
    }

    private void OnDestroy()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(IncreaseSpeed);
        _feedbackManager.OffBeatMissUnsubscribe(DecreaseSpeed);
        _feedbackManager.RepeatedMissUnsubscribe(DecreaseSpeed);
    }

    private IEnumerator IncreaseSpeedCoroutine()
    {
        float elapsedTime = 0.0f;
        float startSpeed = _speed;
        _inTransition = true;

        while (elapsedTime < _lerpDuration)
        {
            _speed = Mathf.Lerp(startSpeed, _speed + _speedModifier, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _speed = _speed + _speedModifier;
        _inTransition = false;
    }

    private void IncreaseSpeed(BeatDirection beatDirection)
    {
        if (_inTransition || _speed >= _maxSpeed)
        {
            return;
        }

        StartCoroutine(IncreaseSpeedCoroutine());
    }

    private IEnumerator DecreaseSpeedCoroutine()
    {
        float elapsedTime = 0.0f;
        float startSpeed = _speed;
        _inTransition = true;

        while (elapsedTime < _lerpDuration)
        {
            _speed = Mathf.Lerp(startSpeed, _speed - _speedModifier, elapsedTime / _lerpDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _speed = _speed - _speedModifier;
        _inTransition = false;
    }

    private void DecreaseSpeed(BeatDirection beatDirection)
    {
        if (_inTransition || _speed <= _minSpeed)
        {
            return;
        }

        StartCoroutine(DecreaseSpeedCoroutine());
    }

    private void MoveRegions()
    {
        foreach (Region region in _regions)
        {
            region.transform.position += Vector3.forward * _speed * Time.deltaTime;

            if (region.transform.position.z > _limitOfMap.position.z)
            {
                if (_isEnding && !_finalIslandReadyToMove)
                {
                    _finalIslandReadyToMove = true;
                }
                else if(!_isEnding)
                {
                    region.transform.position = _startOfMap.position;
                    region.MakeIslandsTransparent();
                }
            }
        }

        if (_finalIslandReadyToMove)
        {
            _finalRegion.position += Vector3.forward * _speed * Time.deltaTime;
        }
    }

    private void Update()
    {
        if (_movementEnabled == false)
        {
            return;
        }

        MoveRegions();
    }

    public void TreadmillWrapUp()
    {
        _isEnding = true;
        _finalRegion.gameObject.SetActive(true);
    }
}