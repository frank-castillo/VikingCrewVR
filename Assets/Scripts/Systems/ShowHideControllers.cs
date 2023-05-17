using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Liminal.SDK.VR.Avatars;

public class ShowHideControllers : MonoBehaviour
{
    [SerializedField] public VRAvatarController PrimaryController;
    public VRAvatarController SecondaryController;

    public bool ShowPrimary;
    public bool ShowSecondary;

    // Update is called once per frame
    void Update()
    {
        if (PrimaryController != null && PrimaryController.gameObject.activeSelf != ShowPrimary)
        {
            PrimaryController.gameObject.SetActive(ShowPrimary);
        }

        if (SecondaryController != null && SecondaryController.gameObject.activeSelf != ShowSecondary)
        {
            SecondaryController.gameObject.SetActive(ShowSecondary);
        }
    }
}
