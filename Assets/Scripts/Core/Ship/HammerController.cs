using System.Collections;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private HammerSide _hammerSide = HammerSide.None;

    [Header("Haptics")]
    [SerializeField] private float _highHapticIntensity = 0.6f;
    [SerializeField] private float _lowHapticIntensity = 0.5f;
    private Coroutine _hapticCoroutine = null;

    [Header("VFX")]
    [SerializeField] private Transform _hammerMeshHolder = null;
    [SerializeField] private Transform _level2Visuals = null;
    [SerializeField] private Transform _level3Visuals = null;
    [SerializeField] private Material _level3Material = null;

    private int _currentLevel = 0;
    private Material _defaultHammerMaterial = null;
    private MeshRenderer _hamerMeshRenderer = null;
    private NoteManager _noteManager = null;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();
        _hamerMeshRenderer = _hammerMeshHolder.GetComponent<MeshRenderer>();

        _defaultHammerMaterial = _hamerMeshRenderer.material;

        LevelUp(1);


    }

    public void LevelEvaluation(BeatDirection beatDirection)
    {
        switch (_noteManager.CurrentTierType)
        {
            case BeatTierType.None:
                break;
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
                Enums.InvalidSwitch(GetType(), _noteManager.CurrentTierType.GetType());
                break;
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

    public void PlayHaptic(HapticIntensity hapticIntensity)
    {
        float intensity = EvaluateHapticIntensity(hapticIntensity);
        OVRInput.Controller controller = EvaluateController();

        if (_hapticCoroutine != null)
        {
            StopCoroutine(_hapticCoroutine);
        }

        _hapticCoroutine = StartCoroutine(HapticFeedbackRoutine(controller, intensity));
    }

    private float EvaluateHapticIntensity(HapticIntensity hapticIntensity)
    {
        switch (hapticIntensity)
        {
            case HapticIntensity.Low:
                return _lowHapticIntensity;
            case HapticIntensity.High:
                return _highHapticIntensity;
            default:
                Enums.InvalidSwitch(GetType(), hapticIntensity.GetType());
                return 0.0f;
        }
    }

    private OVRInput.Controller EvaluateController()
    {
        switch (_hammerSide)
        {
            case HammerSide.Left:
                return OVRInput.Controller.LTouch;
            case HammerSide.Right:
                return OVRInput.Controller.RTouch;
            default:
                Enums.InvalidSwitch(GetType(), _hammerSide.GetType());
                return OVRInput.Controller.All;
        }
    }

    private IEnumerator HapticFeedbackRoutine(OVRInput.Controller controller, float intesity)
    {
        OVRInput.SetControllerVibration(1, intesity, controller);
        yield return new WaitForSecondsRealtime(0.1f);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
