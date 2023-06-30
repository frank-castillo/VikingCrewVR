using System.Collections.Generic;
using UnityEngine;

public class RegionTreadmill : MonoBehaviour
{
    [Header("Tier Speeds")]
    [SerializeField] private float _t1Speed = 10.0f;
    [SerializeField] private float _t2Speed = 20.0f;
    [SerializeField] private float _t3Speed = 35.0f;

    [Header("References")]
    [SerializeField] private Transform _limitOfMap = null;
    [SerializeField] private Transform _startOfMap = null;
    [SerializeField] private Transform _regionFolder = null;
    [SerializeField] private Transform _finalRegion = null;
    [SerializeField] private Transform _islandEmergenceTrigger = null;
    [SerializeField] private Transform _riseBeginPoint = null;
    [SerializeField] private Transform _riseEndPoint = null;

    private NoteManager _noteManager = null;
    private List<Region> _regions = new List<Region>();

    private float _speed = 0.0f;
    private bool _movementEnabled = false;
    private bool _isEnding = false;
    private bool _finalIslandReadyToMove = false;
    private bool _initialized = false;

    public void EnableMovement(bool enable) { _movementEnabled = enable; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _noteManager = ServiceLocator.Get<NoteManager>();

        foreach (Transform child in _regionFolder)
        {
            if (child.gameObject.activeInHierarchy)
            {
                var region = child.GetComponent<Region>();
                region.Initialize(_riseBeginPoint, _riseEndPoint, _islandEmergenceTrigger);
                _regions.Add(region);
            }
        }

        SetupEvents();
        _finalRegion.gameObject.SetActive(false);
        ChangeSpeed(TierType.T1);
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
        _noteManager.SubscribeTierUpgrade(ChangeSpeed);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTierUpgrade(ChangeSpeed);
    }

    private void Update()
    {
        if (_initialized == false || _movementEnabled == false)
        {
            return;
        }

        MoveRegions();
    }

    private void ChangeSpeed(TierType beatTierType)
    {
        switch (beatTierType)
        {
            case TierType.T1:
                _speed = _t1Speed;
                break;
            case TierType.T2:
                _speed = _t2Speed;
                break;
            case TierType.T3:
                _speed = _t3Speed;
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
                break;
        }
    }

    private void MoveRegions()
    {
        foreach (Region region in _regions)
        {
            region.transform.position += Vector3.forward * _speed * Time.deltaTime;

            region.CheckIfIslandIsReadyToEmerge();

            if (region.transform.position.z > _limitOfMap.position.z)
            {
                if (_isEnding && !_finalIslandReadyToMove)
                {
                    _finalIslandReadyToMove = true;
                }
                else if (!_isEnding)
                {
                    region.transform.position = _startOfMap.position;
                    region.ResetIslands();
                }
            }
        }

        if (_finalIslandReadyToMove)
        {
            _finalRegion.position += Vector3.forward * _speed * Time.deltaTime;
        }
    }

    public void TreadmillWrapUp()
    {
        _isEnding = true;
        _finalRegion.gameObject.SetActive(true);
    }
}