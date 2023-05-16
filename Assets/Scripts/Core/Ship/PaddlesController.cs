using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlesController : MonoBehaviour
{
    [Header("SFX Delay")]
    [SerializeField] private float _paddleDelay = 0.0f;
    private Coroutine _paddleSFXCoroutine = null;

    private AudioManager _audioManager = null;
    private List<Animator> _paddleAnimators = new List<Animator>();
    private RowType _paddleState = RowType.None;

    public void SetPaddleAnimation(RowType rowType)
    {
        if (_paddleState == rowType)
        {
            return;
        }
        _paddleState = rowType;

        foreach (Animator animator in _paddleAnimators)
        {
            animator.SetBool("Rowing", EvaluatePaddleState(rowType));
        }

        if (rowType == RowType.StartRowing)
        {
            PlayPaddleSFX();
        }
    }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _audioManager = ServiceLocator.Get<AudioManager>();

        foreach (Transform child in transform)
        {
            Animator paddleAnimator = child.GetComponent<Animator>();
            _paddleAnimators.Add(paddleAnimator);
        }
    }

    private bool EvaluatePaddleState(RowType rowType)
    {
        switch (rowType)
        {
            case RowType.StartRowing:
                return true;
            case RowType.StopRowing:
                return false;
            default:
                Enums.InvalidSwitch(GetType(), rowType.GetType());
                return false;
        }
    }

    private void PlayPaddleSFX()
    {
        if (_paddleSFXCoroutine != null)
        {
            StopCoroutine(_paddleSFXCoroutine);
        }

        _paddleSFXCoroutine = StartCoroutine(PaddleSFXCoroutine());
    }

    private IEnumerator PaddleSFXCoroutine()
    {
        yield return new WaitForSeconds(_paddleDelay);

        _audioManager.PlaySFX(SFXType.PaddleRow);
    }
}