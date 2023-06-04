using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private DrumController _rightDrumController = null;
    [SerializeField] private DrumController _leftDrumController = null;
    [SerializeField] private SailController _sailController = null;
    private FeedbackHandler _feedbackHandler = null;
    private TierToggle _tierToggle = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _tierToggle = GetComponent<TierToggle>();

        _feedbackHandler.Initialize();
        _tierToggle.Initialize();
        _rightDrumController.Initialize();
        _leftDrumController.Initialize();
        _sailController.Initialize();
    }

    public void ShipWrapUp()
    {
        
    }
}