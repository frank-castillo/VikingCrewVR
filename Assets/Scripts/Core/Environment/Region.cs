using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
    private List<IslandMaterialSwapper> _islands = new List<IslandMaterialSwapper>();

    public void Initialize()
    {
        foreach (Transform child in transform)
        {
            var swapper = child.GetComponent<IslandMaterialSwapper>();
            if(swapper.gameObject.activeInHierarchy)
            {
                swapper.Initialize();
                _islands.Add(swapper);
            }
        }
    }

    public void MakeIslandsTransparent()
    {
        foreach (IslandMaterialSwapper child in _islands)
        {
            child.MakeTransparent();
        }
    }
}
