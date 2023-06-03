﻿using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private DrumSide _drumSide = DrumSide.None;

    [Header("Collision")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;
    private float _contactThreshold = 30.0f;

    [Header("VacuumVFX")]
    [SerializeField] private ParticleSystem _vacuumParticles = null;

    private AudioManager _audioManager = null;
    private NoteManager _noteManager = null;
    private FeedbackManager _feedbackManager = null;
    private DrumResponseFeedbackHandler _drumResponseFeedbackHandler = null;
    private bool _initialized = false;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _noteManager = ServiceLocator.Get<NoteManager>();
        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _drumResponseFeedbackHandler = GetComponent<DrumResponseFeedbackHandler>();
        _drumResponseFeedbackHandler.Initialize(_drumSide);

        FeedbackSubscriptions();

        _initialized = true;
    }

    private void FeedbackSubscriptions()
    {
        _feedbackManager.ConstantBeatSubscribe(PlayRuneSFX);
        _feedbackManager.BeatBuildUpSubscribe(PlayVacuum);
    }

    private void OnDestroy()
    {
        if (_initialized == false)
        {
            return;
        }

        _feedbackManager.ConstantBeatUnsubscribe(PlayRuneSFX);
        _feedbackManager.BeatBuildUpUnsubscribe(PlayVacuum);
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

            _noteManager.DrumHit(_drumSide, hammerSde);
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
        if (IsMatchingSideOrBoth(beatDirection) == false)
        {
            return;
        }

        _vacuumParticles.Play();
        _audioManager.PlaySFX(SFXType.DrumVacuum);
    }

    private bool IsMatchingSideOrBoth(BeatDirection beatDirection)
    {
        if (beatDirection == BeatDirection.Both)
        {
            return true;
        }

        if (beatDirection == BeatDirection.Left && _drumSide == DrumSide.Left)
        {
            return true;
        }

        if (beatDirection == BeatDirection.Right && _drumSide == DrumSide.Right)
        {
            return true;
        }

        return false;
    }
}