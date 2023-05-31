using System.Collections;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [Header("Haptic Intensity")]
    [SerializeField] private float _highHapticIntensity = 0.6f;
    [SerializeField] private float _lowHapticIntensity = 0.5f;

    [Header("Tiers")]
    [SerializeField] private NoteSetTier _tier1NoteSets = null;
    [SerializeField] private NoteSetTier _tier2NoteSets = null;
    [SerializeField] private NoteSetTier _tier3NoteSets = null;
    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private bool _recentBeatSuccess = false;

    // Haptics
    private Coroutine _leftHapticCoroutine = null;
    private Coroutine _rightHapticCoroutine = null;

    public void ResetBeatSuccess() { _recentBeatSuccess = false; }

    public void SetBeatManager(BeatManager beatManager) { _beatManager = beatManager; }
    public void SetFeedbackManager(FeedbackManager feedbackManager) { _feedbackManager = feedbackManager; }

    public NoteManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    public void DrumHit(HammerSide hammerType)
    {
        if (_beatManager.IsOnBeat || _beatManager.PreHitWindowCheck())
        {
            HitOnBeat(hammerType);
        }
        else
        {
            HitOffBeat(hammerType);
        }
    }

    private void HitOnBeat(HammerSide hammerType)
    {
        if (_recentBeatSuccess == false)
        {
            //_feedbackManager.OnFirstBeatFeedback();
        }
        else
        {
            //_feedbackManager.OnMinorBeatFeedback();
        }

        PlayHaptic(hammerType, _highHapticIntensity);

        _recentBeatSuccess = true;
    }

    private void HitOffBeat(HammerSide hammerType)
    {
        _feedbackManager.OffBeatFeedback(HammerSideToDirection(hammerType));
        PlayHaptic(hammerType, _lowHapticIntensity);
    }

    private void PlayHaptic(HammerSide hammerType, float intensity)
    {
        switch (hammerType)
        {
            case HammerSide.Left:
                PlayLeftHaptic(intensity);
                break;
            case HammerSide.Right:
                PlayRightHaptic(intensity);
                break;
            default:
                Enums.InvalidSwitch(GetType(), hammerType.GetType());
                break;
        }
    }

    private void PlayLeftHaptic(float intensity)
    {
        if (_leftHapticCoroutine != null)
        {
            StopCoroutine(_leftHapticCoroutine);
        }

        _leftHapticCoroutine = StartCoroutine(HapticFeedbackRoutine(OVRInput.Controller.LTouch, intensity));
    }

    private void PlayRightHaptic(float intensity)
    {
        if (_rightHapticCoroutine != null)
        {
            StopCoroutine(_leftHapticCoroutine);
        }

        _rightHapticCoroutine = StartCoroutine(HapticFeedbackRoutine(OVRInput.Controller.RTouch, intensity));
    }

    private IEnumerator HapticFeedbackRoutine(OVRInput.Controller controller, float intesity)
    {
        OVRInput.SetControllerVibration(1, intesity, controller);
        yield return new WaitForSecondsRealtime(0.1f);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    private BeatDirection HammerSideToDirection(HammerSide hammerType)
    {
        switch (hammerType)
        {
            case HammerSide.Left:
                return BeatDirection.Left;
            case HammerSide.Right:
                return BeatDirection.Right;
            default:
                Enums.InvalidSwitch(GetType(), hammerType.GetType());
                return BeatDirection.None;
        }
    }
}
