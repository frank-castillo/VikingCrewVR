using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }
}