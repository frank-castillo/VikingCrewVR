using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;
    private float _contactThreshold = 30.0f;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _vacuumParticles = null;
    [SerializeField] private ParticleSystem _failureVFX = null;
    [SerializeField] private UnityEventFeedback _successVFX = null;

    [Header("References")]
    [SerializeField] private DrumEmitter _drumEmitter = null;

    private AudioManager _audioManager = null;
    private NoteManager _noteManager = null;
    private FeedbackManager _feedbackManager = null;
    private FeedbackHandler _feedbackHandler = null;
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

        _feedbackHandler = GetComponent<FeedbackHandler>();

        _feedbackHandler.Initialize();
        _drumEmitter.Initialize();

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

    private void PlayRuneSFX()
    {
        _audioManager.PlaySFX(SFXType.DrumHum);
    }

    private void PlayVacuum()
    {
        _vacuumParticles.Play();
        _audioManager.PlaySFX(SFXType.DrumVacuum);
    }

    private void PlaySuccessVFX()
    {
        _successVFX.Play();
    }

    public void EmitParticle()
    {
        _drumEmitter.ActivateParticle();
    }
}