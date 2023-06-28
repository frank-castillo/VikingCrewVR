using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private DrumController _drum = null;
    [SerializeField] private SailController _sailController = null;
    [SerializeField] private Animator dragonHeadAnimator = null;
    [SerializeField] private List<AnimationFeedback> _vikingsRowing = new List<AnimationFeedback>();
    [SerializeField] private List<Feedback> _rowingSoundFeedback = new List<Feedback>();
    private FeedbackHandler _feedbackHandler = null;
    private TierToggle _tierToggle = null;

    public DrumController Drum { get => _drum; }

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _tierToggle = GetComponent<TierToggle>();

        _feedbackHandler.Initialize();
        _tierToggle.Initialize();
        _drum.Initialize();
        _sailController.Initialize();

        foreach (var feedback in _rowingSoundFeedback)
        {
            feedback.Initialize();
        }
    }

    public void ShipWrapUp()
    {
        dragonHeadAnimator.SetTrigger("Bow");
    }

    public void Row()
    {
        foreach (var rowFeedback in _vikingsRowing)
        {
            rowFeedback.Play();
        }

        foreach (var soundFeedback in _rowingSoundFeedback)
        {
            soundFeedback.Play();
        }
    }
}