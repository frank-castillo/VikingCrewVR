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
    private List<Transform> _regions = new List<Transform>();

    private FeedbackManager _feedbackManager = null;
    private bool _movementEnabled = false;
    private float _speed = 5.0f;

    public void EnableMovement(bool enable) { _movementEnabled = enable; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        foreach (Transform region in _regionFolder)
        {
            _regions.Add(region);
        }

        _speed = _minSpeed;

        _feedbackManager.OnBeatHitSubscribe(IncreaseSpeed);
        _feedbackManager.OffBeatMissSubscribe(DecreaseSpeed);
    }

    private void OnDisable()
    {
        _feedbackManager.OnBeatHitUnsubscribe(IncreaseSpeed);
        _feedbackManager.OffBeatMissUnsubscribe(DecreaseSpeed);
    }

    private void IncreaseSpeed()
    {
        if(_speed + _speedModifier > _maxSpeed)
        {
            _speed = _maxSpeed;
        }
        else
        {
            _speed += _speedModifier;
        }
    }

    private void DecreaseSpeed()
    {
        if (_speed - _speedModifier < _minSpeed)
        {
            _speed = _minSpeed;
        }
        else
        {
            _speed -= _speedModifier;
        }
    }

    void Update()
    {
        if(!_movementEnabled)
        {
            return;
        }

        foreach (Transform region in _regions)
        {
            region.position += Vector3.forward * _speed * Time.deltaTime;

            if (region.position.z > _limitOfMap.position.z)
            {
                region.position = _startOfMap.position;
            }
        }
    }
}