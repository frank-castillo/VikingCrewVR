﻿using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Beat Timers")]
    [SerializeField] private float _beatDelay = 0.75f;
    [SerializeField] private float _beatBuildUp = 0.75f;
    [SerializeField] private float _preHitWindowDelay = 0.2f; // On pre side
    [SerializeField] private float _postHitWindowDelay = 0.2f; // On end side

    [Header("Delay")]
    [SerializeField] private float _initialDelay = 5.0f;

    private NoteManager _noteManager = null;
    private Ship _ship = null;
    private DrumController _rightDrum = null;
    private DrumController _leftDrum = null;
    private float _beatTimer = 0.0f;
    private float _hitWindowTimer = 0.0f;
    private bool _isOnBeat = false;
    private bool _beatBuildUpPlayed = false;
    private bool _beatEnabled = false;

    public bool IsOnBeat { get => _isOnBeat; }

    public void SetNoteManager(NoteManager noteManager) { _noteManager = noteManager; }
    public void SetShip(Ship ship) { _ship = ship; }
    public void SetBeatEnabled(bool enabled) { _beatEnabled = enabled; }

    public void SetDrums(DrumController rightDrum, DrumController leftDrum)
    {
        _rightDrum = rightDrum;
        _leftDrum = leftDrum;
    }

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }

    private void Update()
    {
        if (_beatEnabled == false)
        {
            return;
        }

        _beatTimer -= Time.deltaTime;

        EvaluateBeatBuildUp();

        if (_beatTimer < 0)
        {
            Beat();
        }
        else if (_isOnBeat)
        {
            EvaluateHitWindow();
        }
    }

    public void StartBeat()
    {
        _beatTimer = _beatDelay;
        _beatEnabled = true;
    }

    private void EvaluateBeatBuildUp()
    {
        if (_beatBuildUpPlayed == true)
        {
            return;
        }

        if (_beatTimer <= _beatBuildUp)
        {
            _beatBuildUpPlayed = true;

            if (_beatEnabled)
            {
                _noteManager.PreBeat();
            }
        }
    }

    private void Beat()
    {
        _isOnBeat = true;
        _beatBuildUpPlayed = false;

        _noteManager.NoteBeat();

        _beatTimer = _beatDelay;
        _hitWindowTimer = _postHitWindowDelay;
    }

    private void EvaluateHitWindow()
    {
        _hitWindowTimer -= Time.deltaTime;

        if (_hitWindowTimer < 0)
        {
            _isOnBeat = false;
            ResetDrums();

            _noteManager.EndOfBeat();
        }
    }

    private void ResetDrums()
    {
        _leftDrum.SetRecentlyHit(false);
        _rightDrum.SetRecentlyHit(false);
    }

    public bool PreHitWindowCheck()
    {
        float delta = _beatTimer - _preHitWindowDelay;
        return delta < 0;
    }
}