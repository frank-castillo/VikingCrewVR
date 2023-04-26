using UnityEngine;

public class Drum : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BeatManager _beatManager = null;

    [Header("Collider Layer Integer Reference")]
    [SerializeField] private int _hammerLayer = 6;

    public Drum Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == _hammerLayer)
        {
            _beatManager.DrumHit();
        }
    }
}