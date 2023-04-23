using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Drum _drum = null;

    public Ship Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _drum.Initialize();

        return this;
    }
}