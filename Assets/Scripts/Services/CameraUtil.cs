using UnityEngine;

public class CameraUtil : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera = null;

    public Camera Camera { get => _mainCamera; }

    public CameraUtil Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }
}
