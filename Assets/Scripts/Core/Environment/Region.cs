using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    private Transform _riseBeginPoint = null;
    private Transform _riseEndPoint = null;
    private Transform _emergePoint = null;
    private List<RisingIsland> _islands = new List<RisingIsland>();

    public void Initialize(Transform riseBeginPoint, Transform riseEndPoint, Transform emergePoint)
    {
        _riseBeginPoint = riseBeginPoint;
        _riseEndPoint = riseEndPoint;
        _emergePoint = emergePoint;

        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                var island = child.GetComponent<RisingIsland>();
                island.Initialize(_riseBeginPoint.position.y, _riseEndPoint.position.y, emergePoint);
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
