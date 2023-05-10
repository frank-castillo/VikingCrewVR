using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailController : MonoBehaviour
{
    [SerializeField] private Animator _sailAnimator = null;
    [SerializeField] private const string _animationName = "X";
    public void PlaySailAnimation()
    {
        _sailAnimator.Play(_animationName);
    }
}
