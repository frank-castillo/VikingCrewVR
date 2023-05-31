using UnityEngine;

public class HammerController : MonoBehaviour
{
    [SerializeField] private Transform _hammerMeshHolder = null;
    [SerializeField] private Transform _level2Visuals = null;
    [SerializeField] private Transform _level3Visuals = null;
    [SerializeField] private Material _level3Material = null;

    private int _currentLevel = 0;
    private Material _defaultHammerMaterial = null;
    private MeshRenderer _hamerMeshRenderer = null;
    private BeatManager _beatManager = null;

    public void Initialize()
    {
        _beatManager = ServiceLocator.Get<BeatManager>();
        _hamerMeshRenderer = _hammerMeshHolder.GetComponent<MeshRenderer>();

        _defaultHammerMaterial = _hamerMeshRenderer.material;

        LevelUp(1);
    }

    public void LevelEvaluation(BeatDirection beatDirection)
    {
        if (_beatManager.CurrentTier == BeatTierType.T1)
        {
            LevelUp(1);
        }
        else if (_beatManager.CurrentTier == BeatTierType.T2)
        {
            LevelUp(2);
        }
        else if (_beatManager.CurrentTier == BeatTierType.T3)
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
