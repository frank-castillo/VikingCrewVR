using System.Collections.Generic;
using UnityEngine;

public class CrewController : MonoBehaviour
{
    [Header("Sleep Data")]
    [SerializeField] private float _sleepDelay = 5.0f;
    [SerializeField] private float _sleepVariance = 3.0f;

    [Header("References")]
    [SerializeField] private Transform _vikingsFolder = null;

    private FeedbackManager _feedbackManager = null;
    private List<VikingBehavior> _vikings = new List<VikingBehavior>();

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        InitializeVikings();

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _feedbackManager.OnBeatHitSubscribe(StartRow);
        _feedbackManager.OffBeatMissSubscribe(StopRowing);
        _feedbackManager.RepeatedMissSubscribe(StopRowing);
    }

    private void OnDestroy()
    {
        _feedbackManager.OnBeatHitUnsubscribe(StartRow);
        _feedbackManager.OffBeatMissUnsubscribe(StopRowing);
        _feedbackManager.RepeatedMissUnsubscribe(StopRowing);
    }

    public void StartDefaultCrewBehavior()
    {
        foreach (VikingBehavior viking in _vikings)
        {
            viking.StartDefaultBehavior();
        }
    }

    private void InitializeVikings()
    {
        foreach (Transform child in _vikingsFolder)
        {
            VikingBehavior viking = child.GetComponent<VikingBehavior>();
            viking.Initialize(_sleepDelay, _sleepVariance);
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
