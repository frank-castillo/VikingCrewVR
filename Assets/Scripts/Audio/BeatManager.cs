using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private FeedbackManager _feedbackManager = null;

    public BeatManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager.Initialize();

        return this;
    }
}