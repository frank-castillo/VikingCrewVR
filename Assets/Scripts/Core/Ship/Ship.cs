using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private DrumController _drumController = null;

    public Ship Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _drumController.Initialize();

        return this;
    }
}