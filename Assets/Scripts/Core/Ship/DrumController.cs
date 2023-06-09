using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private DrumSide _drumSide = DrumSide.None;

    [Header("Collision")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;
    private float _contactThreshold = 30.0f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _vacuumParticles = null;
    [SerializeField] private UnityEventFeedback _successVFX = null;

    private AudioManager _audioManager = null;
    private NoteManager _noteManager = null;
    private FeedbackManager _feedbackManager = null;
    private DrumResponseFeedbackHandler _drumResponseFeedbackHandler = null;
    private bool _initialized = false;
    private bool _recentlyHit = false;

    public bool RecentlyHit { get => _recentlyHit; }

    public void SetRecentlyHit(bool recentlyHit) { _recentlyHit = recentlyHit; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _noteManager = ServiceLocator.Get<NoteManager>();
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _drumResponseFeedbackHandler = GetComponent<DrumResponseFeedbackHandler>();
        _drumResponseFeedbackHandler.Initialize(_drumSide);

        FeedbackSubscriptions();

        _recentlyHit = false;
        _initialized = true;
    }

    private void FeedbackSubscriptions()
    {
        _feedbackManager.SubscribeConstantBeat(PlayRuneSFX);
        _feedbackManager.SubscribeBeatBuildUp(PlayVacuum);
        _feedbackManager.SubscribeOnBeatFirstHit(PlaySuccessVFX);
    }

    private void OnDestroy()
    {
        if (_initialized == false)
        {
            return;
        }

        _feedbackManager.UnsubscribeConstantBeat(PlayRuneSFX);
        _feedbackManager.UnsubscribeBeatBuildUp(PlayVacuum);
        _feedbackManager.UnsubscribeOnBeatFirstHit(PlaySuccessVFX);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (IsValidCollision(collision) == false)
        {
            return;
        }

        if (collision.gameObject.layer == (uint)_hammerLayer)
        {
            HammerSide hammerSde = HammerSide.None;

            if (collision.gameObject.CompareTags("LHand"))
            {
                hammerSde = HammerSide.Left;
            }
            else if (collision.gameObject.CompareTags("RHand"))
            {
                hammerSde = HammerSide.Right;
            }

            _noteManager.DrumHit(hammerSde);
        }
    }

    private bool IsValidCollision(Collision collision)
    {
        Vector3 validDirection = Vector3.down;
        float hitAngle = Vector3.Angle(collision.GetContact(0).normal, validDirection);

        if (hitAngle <= _contactThreshold)
        {
            return true;
        }

        return false;
    }

    private void PlayRuneSFX(BeatDirection beatDirection)
    {
        _audioManager.PlaySFX(SFXType.DrumHum);
    }

    private void PlayVacuum(BeatDirection beatDirection)
    {
        _vacuumParticles.Play();
        _audioManager.PlaySFX(SFXType.DrumVacuum);
    }

    private void PlaySuccessVFX(BeatDirection beatDirection)
    {
        _successVFX.Play();
    }
}