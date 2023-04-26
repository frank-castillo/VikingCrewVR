using UnityEngine;

public class Drum : MonoBehaviour
{
    public Drum Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }
}