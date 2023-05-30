﻿using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;
    private float _contactThreshold = 30.0f;

    [Header("VacuumVFX")]
    [SerializeField] private ParticleSystem _vacuumParticles = null;

    private AudioManager _audioManager = null;
    private NoteManager _noteManager = null;
    private FeedbackManager _feedbackManager = null;
    private FeedbackHandler _feedbackHandler = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();
        _noteManager = ServiceLocator.Get<NoteManager>();

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _feedbackHandler.Initialize();

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        FeedbackSubscriptions();
    }

    private void FeedbackSubscriptions()
    {
        _feedbackManager.ConstantBeatSubscribe(PlayRuneSFX);
        _feedbackManager.BeatBuildUpSubscribe(PlayVacuum);
    }

    private void OnDestroy()
    {
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
            HammerType hammerType = HammerType.None; 

            if (collision.gameObject.CompareTags("LHand"))
            {
                hammerType = HammerType.Left;
            }
            else if (collision.gameObject.CompareTags("RHand"))
            {
                hammerType = HammerType.Right;
            }

            _noteManager.DrumHit(hammerType);
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

}