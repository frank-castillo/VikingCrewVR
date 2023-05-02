using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrumController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatManager _beatManager = null;

    [Header("Collider Layer Integer Reference")]
    [SerializeField] private int _hammerLayer = 6;

    public DrumController Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _hammerLayer)
        {
            _beatManager.DrumHit();
        }
    }
}