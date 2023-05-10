using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailController : MonoBehaviour
{
    [SerializeField] private Animator _sailAnimator = null;
    [SerializeField] private Transform _sailParent = null;
    [SerializeField] private const string _animationName = "Sail";

    public void PlaySailAnimation()
    {
        _sailParent.gameObject.SetActive(true);
        _sailAnimator.Play(_animationName);
    }
}
