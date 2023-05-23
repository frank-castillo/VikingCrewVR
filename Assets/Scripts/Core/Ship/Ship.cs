using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private DrumController _drumController = null;
    [SerializeField] private CrewController _crewController = null;
    [SerializeField] private SailController _sailController = null;
    [SerializeField] private FeedbackHandler _feedbackHandler = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _drumController.Initialize();
        _crewController.Initialize();
        _feedbackHandler.Initialize();
        _sailController.Initialize();
    }

    public void StartShip()
    {
        _crewController.StartDefaultCrewBehavior();
    }

    public void ShipWrapUp()
    {
        _sailController.PlaySailAnimation();
    }
}