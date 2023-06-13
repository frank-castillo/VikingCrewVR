using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    private List<RisingIsland> _islands = new List<RisingIsland>();

    public void Initialize(Transform riseBeginPoint, Transform riseEndPoint, Transform emergePoint)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                var island = child.GetComponent<RisingIsland>();
                island.Initialize(riseBeginPoint.position.y, riseEndPoint.position.y, emergePoint);
                _islands.Add(island);
            }
        }
    }

    public void ResetIslands()
    {
        foreach (RisingIsland island in _islands)
        {
            island.ResetInitialPosition();
        }
    }

    public void CheckIfIslandIsReadyToEmerge()
    {
        foreach (RisingIsland island in _islands)
        {
            island.EvaluateIfReadyToEmerge();
        }
    }
}
