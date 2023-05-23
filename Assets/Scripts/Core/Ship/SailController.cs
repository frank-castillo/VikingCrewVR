using UnityEngine;

public class SailController : MonoBehaviour
{
    [SerializeField] private RunesProgressionController _runesProgressionController = null;
    [SerializeField] private Animator _sailAnimator = null;
    [SerializeField] private Transform _sailParent = null;
    private const string _animationName = "Sail";

    public void Initialize()
    {
        _runesProgressionController.Initialize();
    }

    public void PlaySailAnimation()
    {
        _sailParent.gameObject.SetActive(true);
        _sailAnimator.Play(_animationName);
    }
}
