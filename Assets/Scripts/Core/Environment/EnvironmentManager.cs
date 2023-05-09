using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    [SerializeField] private RegionTreadmill _environmentTreadmill = null;
    [SerializeField] private WindController _windController = null;


    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        if (_environmentTreadmill != null)
        {
            _environmentTreadmill.Initialize();
        }

        if (_windController != null)
        {
            _windController.Initialize();
        }
    }

    public void StartEnvironment()
    {
        _environmentTreadmill.EnableMovement(true);
    }
}