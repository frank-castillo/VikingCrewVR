using System.Collections.Generic;
using UnityEngine;

public class CrewController : MonoBehaviour
{
    [SerializeField] private Transform _vikingsFolder = null;
    [SerializeField] private CrewFeedback _crewFeedback = null;
    private FeedbackHandler _feedbackHandler = null;
    private List<VikingBehavior> _vikings = new List<VikingBehavior>();

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        SetupVikings();

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _feedbackHandler.Initialize();
    }

    private void SetupVikings()
    {
        foreach (Transform child in _vikingsFolder)
        {
            VikingBehavior viking =  child.GetComponent<VikingBehavior>();
            viking.Initialize();
            _vikings.Add(viking);
        }

        _crewFeedback.SetVikings(_vikings);
    }
}
