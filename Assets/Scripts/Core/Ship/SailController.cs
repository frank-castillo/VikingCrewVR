using UnityEngine;

public class SailController : MonoBehaviour
{
    [SerializeField] private RunesProgressionController _leftRunesProgressionController = null;
    [SerializeField] private RunesProgressionController _rightRunesProgressionController = null;

    [SerializeField] private Animator _sailAnimator = null; 
    [SerializeField] private Transform _sailParent = null;
    private const string _animationName = "Sail";

    public void Initialize()
    {
        _leftRunesProgressionController.Initialize();
        _rightRunesProgressionController.Initialize();
    }

    public void PlaySailAnimation()
    {
        _sailParent.gameObject.SetActive(true);
        _sailAnimator.Play(_animationName);
    }
}
