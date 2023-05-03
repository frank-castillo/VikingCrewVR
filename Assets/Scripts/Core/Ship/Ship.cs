using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private DrumController _drumController = null;
    [SerializeField] private CrewController _crewController = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _drumController.Initialize();
        _crewController.Initialize();
    }
}