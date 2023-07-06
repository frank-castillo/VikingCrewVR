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

    private NoteManager _noteManager = null;
    private Material _defaultHammerMaterial = null;
    private MeshRenderer _hamerMeshRenderer = null;
    private int _currentLevel = 0;
    private bool _initialized = false;

    [Header("Hammer Smoothing")]
    [SerializeField] private Transform _hammerTransformTarget = null; // The transform the hand smoothly lerps towards
    [Range(0.0f, 1.0f)] [SerializeField] private float _smoothingFactor = 0.85f; // Adjust this value to control the amount of smoothing on hammer to controller lerping

    // Data for lerping hammer to controller
    private Vector3 _smoothedHammerPosition = Vector3.zero;
    private Quaternion _smoothedHammerRotation = Quaternion.identity;

    public void Initialize()
    {
#if UNITY_EDITOR
        if (_hammerSide == HammerSide.Left)
        {
            this.gameObject.SetActive(false);
            return;
        }
#endif

        _noteManager = ServiceLocator.Get<NoteManager>();
        _hamerMeshRenderer = _hammerMeshHolder.GetComponent<MeshRenderer>();

        _defaultHammerMaterial = _hamerMeshRenderer.material;

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

    private void Update()
    {
        if (_initialized == false)
        {
            return;
        }
        UpdateHandTransform();
    }

    private void SetupEvents()
    {
        _noteManager.SubscribeTierUpgrade(LevelEvaluation);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTierUpgrade(LevelEvaluation);
    }

    public void LevelEvaluation(TierType beatTierType)
    {
        switch (beatTierType)
        {
            case TierType.T1:
                LevelUp(1);
                break;
            case TierType.T2:
                LevelUp(2);
                break;
            case TierType.T3:
                LevelUp(3);
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
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

    private void UpdateHandTransform()
    {
        // Update the current hand position and rotation
        Vector3 _currentControllerPosition = _hammerTransformTarget.position;
        Quaternion _currentControllerRotation = _hammerTransformTarget.rotation;

        // Apply low-pass filter to smooth hand movement
        _smoothedHammerPosition = Vector3.Lerp(_smoothedHammerPosition, _currentControllerPosition, _smoothingFactor);
        _smoothedHammerRotation = Quaternion.Slerp(_smoothedHammerRotation, _currentControllerRotation, _smoothingFactor);

        // Use the smoothed position and rotation for rendering or other purposes
        transform.position = _smoothedHammerPosition;
        transform.rotation = _smoothedHammerRotation;
    }
}
