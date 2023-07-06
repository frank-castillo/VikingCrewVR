using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private RegionTreadmill _environmentTreadmill = null;
    [SerializeField] private WindController _windController = null;
    [SerializeField] private FogController _fogController = null;
    [SerializeField] private FinalRegionGetter _finalRegion = null;
    private FeedbackHandler _feedbackHandler = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackHandler = GetComponent<FeedbackHandler>();

        _feedbackHandler.Initialize();
        _environmentTreadmill.Initialize();
        _windController.Initialize();
        _fogController.Initialize();
        _finalRegion.Initialize();
    }

    public void StartEnvironment()
    {
        _environmentTreadmill.EnableMovement(true);
        _fogController.ChangeFog(TierType.T1);
    }

    public void EnvironmentWrapUp()
    {
        _environmentTreadmill.TreadmillWrapUp();
        _fogController.WrapUpFog();
    }
}