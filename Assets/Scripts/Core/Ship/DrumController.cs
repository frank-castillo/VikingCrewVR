﻿using System;
using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("Collision")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;
    private float _contactThreshold = 30.0f;

    private BeatManager _beatManager = null;
    private FeedbackManager _feedbackManager = null;
    private FeedbackHandler _feedbackHandler = null;

    [SerializeField] private HitSparksFeedback _sparksFeedback = null;
    [SerializeField] private ParticleSystem _vacuumParticles = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _beatManager = ServiceLocator.Get<BeatManager>();

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _feedbackHandler.Initialize();

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();
        _feedbackManager.BeatBuildUpSubscribe(PlayVacuum);
    }

    private void OnDestroy()
    {
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
            if (collision.gameObject.CompareTags("LHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.LTouch);
            }
            else if (collision.gameObject.CompareTags("RHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.RTouch);
            }

            _sparksFeedback.SetCollisionData(collision);
            _beatManager.DrumHit();
        }
    }

    private bool IsValidCollision(Collision collision)
    {
        Vector3 validDirection = Vector3.down;
        float hitAngle = Vector3.Angle(collision.GetContact(0).normal, validDirection);

        Debug.Log($"Hit angle: {hitAngle}");

        if (hitAngle <= _contactThreshold)
        {
            return true;
        }

        return false;
    }

    private void PlayVacuum()
    {
        _vacuumParticles.Play();
    }
}