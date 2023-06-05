using Liminal.SDK.VR.Avatars;
using UnityEngine;

public class ShowHideControllers : MonoBehaviour
{
    [SerializeField] private VRAvatarController _primaryController = null;
    [SerializeField] private VRAvatarController _secondaryController = null;

    [SerializeField] private bool _showPrimary = true;
    [SerializeField] private bool _showSecondary = true;

    private void Awake()
    {
        if (_primaryController != null && _primaryController.gameObject.activeSelf != _showPrimary)
        {
            _primaryController.gameObject.SetActive(_showPrimary);
        }

        if (_secondaryController != null && _secondaryController.gameObject.activeSelf != _showSecondary)
        {
            _secondaryController.gameObject.SetActive(_showSecondary);
        }
    }

    public void HideHammers()
    {
        _primaryController.transform.parent.gameObject.SetActive(false);
        _secondaryController.transform.parent.gameObject.SetActive(false);
    }
}
