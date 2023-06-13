using System.Collections.Generic;
using UnityEngine;

public class TierToggle : MonoBehaviour
{
    [Header("Tier Upgrade Objects")]
    [SerializeField] private List<GameObject> _t2Objects = new List<GameObject>();
    [SerializeField] private List<GameObject> _t3Objects = new List<GameObject>();
    private NoteManager _noteManager = null;
    private bool _initialized = false;

    public void Initialize()
    {
        _noteManager = ServiceLocator.Get<NoteManager>();

        SubscribeEvents();

        _initialized = true;

        EnableObjectList(false, _t2Objects);
        EnableObjectList(false, _t3Objects);
    }

    private void OnDestroy()
    {
        if (_initialized == false)
        {
            return;
        }

        UnsubscribeEvents();
    }


    private void SubscribeEvents()
    {
        _noteManager.SubscribeTierUpgrade(TierUpgradeToggle);
    }

    private void UnsubscribeEvents()
    {
        _noteManager.UnsubscribeTierUpgrade(TierUpgradeToggle);
    }

    private void TierUpgradeToggle(BeatTierType beatTierType)
    {
        switch (beatTierType)
        {
            case BeatTierType.T1:
                break;
            case BeatTierType.T2:
                EnableObjectList(true, _t2Objects);
                break;
            case BeatTierType.T3:
                EnableObjectList(true, _t3Objects);
                break;
            default:
                Enums.InvalidSwitch(GetType(), beatTierType.GetType());
                break;
        }
    }

    private void EnableObjectList(bool enable, List<GameObject> list)
    {
        foreach (GameObject gameObject in list)
        {
            gameObject.SetActive(enable);
        }
    }
}
