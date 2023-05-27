using System.Collections.Generic;
using UnityEngine;

public class CrewController : MonoBehaviour
{
    [Header("Sleep Data")]
    [SerializeField] private float _sleepDelay = 5.0f;
    [SerializeField] private float _sleepVariance = 3.0f;

    [Header("References")]
    [SerializeField] private Transform _vikingsFolder = null;
    [SerializeField] private PaddlesController _paddlesController = null;

    private FeedbackManager _feedbackManager = null;
    private FeedbackHandler _feedbackHandler = null;
    private List<VikingBehavior> _vikings = new List<VikingBehavior>();

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackManager = ServiceLocator.Get<FeedbackManager>();

        _feedbackHandler = GetComponent<FeedbackHandler>();

        InitializeVikings();
        _paddlesController.Initialize();
        _feedbackHandler.Initialize();

        _feedbackManager.OnBeatFirstHitSubscribe(StartRow);
        _feedbackManager.OffBeatMissSubscribe(StopRowing);
        _feedbackManager.RepeatedMissSubscribe(StopRowing);
    }

    private void InitializeVikings()
    {
        foreach (Transform child in _vikingsFolder)
        {
            VikingBehavior viking = child.GetComponent<VikingBehavior>();
            viking.Initialize(this, _sleepDelay, _sleepVariance);
            _vikings.Add(viking);
        }
    }

    private void OnDestroy()
    {
        _feedbackManager.OnBeatFirstHitUnsubscribe(StartRow);
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

    public void SetPaddleAnimation(RowType rowType)
    {
        _paddlesController.SetPaddleAnimation(rowType);
    }
}
