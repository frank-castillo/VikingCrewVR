using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEvolutionController : MonoBehaviour
{
    [SerializeField] private int _currentLevel = -1;
    [SerializeField] private Transform _hammerMeshHolder = null;
    [SerializeField] private Transform _level2Visuals = null;
    [SerializeField] private Transform _level3Visuals = null;

    [SerializeField] private Material _level3Material = null;
    private Material _defaultHammerMaterial = null;
    private MeshRenderer _hamerMeshRenderer = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _currentLevel = 1;
        _hamerMeshRenderer = _hammerMeshHolder.GetComponent<MeshRenderer>();
        _defaultHammerMaterial = _hamerMeshRenderer.material;
    }

    public void HandleStreakUpdate(int streakValue)
    {
        if (streakValue <= 10)
        {
            LevelUp(1);
        }
        else if (streakValue > 10 && streakValue < 20)
        {
            LevelUp(2);
        }
        else if (streakValue > 20 && streakValue <= 30)
        {
            LevelUp(3);
        }
    }

    private void LevelUp(int newLevel)
    {
        if (_currentLevel == newLevel)
        {
            return;
        }

        _currentLevel = newLevel;

        if (newLevel == 1)
        {
            _level3Visuals.gameObject.SetActive(false);
            _level2Visuals.gameObject.SetActive(false);
            SwapHammerMaterial(_defaultHammerMaterial);
        }
        else if (newLevel == 2)
        {
            _level3Visuals.gameObject.SetActive(false);
            _level2Visuals.gameObject.SetActive(true);
            SwapHammerMaterial(_defaultHammerMaterial);
        }
        else if (newLevel == 3)
        {
            _level3Visuals.gameObject.SetActive(true);
            _level2Visuals.gameObject.SetActive(false);
            SwapHammerMaterial(_level3Material);
        }
    }

    private void SwapHammerMaterial(Material newMaterial)
    {
        _hamerMeshRenderer.material = newMaterial;
    }
}
