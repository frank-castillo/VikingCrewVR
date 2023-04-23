using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }
}