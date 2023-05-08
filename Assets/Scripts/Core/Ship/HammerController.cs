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
    private FeedbackHandler _feedbackHandler = null;

    public void Initialize()
    {
        _beatManager = ServiceLocator.Get<BeatManager>();

        _hamerMeshRenderer = _hammerMeshHolder.GetComponent<MeshRenderer>();

        _currentLevel = 1;
        _defaultHammerMaterial = _hamerMeshRenderer.material;
    }

    public void LevelEvaluation(int streakValue)
    {
        Debug.Log($"Streak [{streakValue}]");

        if (streakValue < _beatManager.TierTwo)
        {
            return;
        }
        else if (streakValue < _beatManager.TierThree)
        {
            LevelUp(2);
        }
        else
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
