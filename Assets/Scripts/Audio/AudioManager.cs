using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager Initialize()
    {
        Debug.Log($"<color=Cyan> {this.GetType()} starting setup. </color>");

        return this;
    }
}