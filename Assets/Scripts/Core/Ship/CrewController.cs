using System.Collections.Generic;
using UnityEngine;

public class CrewController : MonoBehaviour
{
    [SerializeField] private Transform _vikingsFolder = null;
    private List<VikingBehavior> _vikings = new List<VikingBehavior>();

    private FeedbackManager _feedbackManager = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        SetupVikings();

        _feedbackManager.OnBeatHitSubscribe(StartRow);
        _feedbackManager.OffBeatMissSubscribe(StopRowing);
    }

    private void OnDisable()
    {
        _feedbackManager.OnBeatHitUnsubscribe(StartRow);
        _feedbackManager.OffBeatMissUnsubscribe(StopRowing);
    }

    private void SetupVikings()
    {
        foreach (Transform child in _vikingsFolder)
        {
            VikingBehavior viking =  child.GetComponent<VikingBehavior>();
            viking.Initialize();
            _vikings.Add(viking);
        }
    }

    private void StartRow()
    {
        Rowing(RowType.StartRowing);
    }

    private void StopRowing()
    {
        Rowing(RowType.StopRowing);
    }

    private void Rowing(RowType rowType)
    {
        foreach (VikingBehavior viking in _vikings)
        {
            viking.RowEvent(rowType);
        }
    }
}
