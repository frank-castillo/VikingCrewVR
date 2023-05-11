using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private RegionTreadmill _environmentTreadmill = null;
    [SerializeField] private WindController _windController = null;
    private FeedbackHandler _feedbackHandler = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackHandler = GetComponent<FeedbackHandler>();

        _feedbackHandler.Initialize();
        _environmentTreadmill.Initialize();
        _windController.Initialize();
    }

    public void StartEnvironment()
    {
        _environmentTreadmill.EnableMovement(true);
    }
}