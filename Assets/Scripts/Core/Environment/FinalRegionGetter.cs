﻿using UnityEngine;

public class FinalRegionGetter : MonoBehaviour
{
    [SerializeField] private Transform _experienceEndingTrigger = null;
    [SerializeField] private Transform _triggeringNode = null;

    private LevelLoader _levelLoader = null;
    private bool _experienceEnded = false;

    public void Initialize()
    {
        _levelLoader = ServiceLocator.Get<LevelLoader>();
    }

    private void Update()
    {
        if(_experienceEnded)
        {
            return;
        }

        if (_experienceEndingTrigger.position.z >= _triggeringNode.position.z)
        {
            _experienceEnded = true;
            _levelLoader.FinalizeExperience();
        }
    }
}