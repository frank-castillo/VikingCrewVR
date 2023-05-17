using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR.Avatars;

public class ShowHideControllers : MonoBehaviour
{
    [SerializeField] private VRAvatarController _primaryController;
    [SerializeField] private VRAvatarController _secondaryController;

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
