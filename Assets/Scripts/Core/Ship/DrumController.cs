using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("Layer References")]
    [SerializeField] private LayerType _hammerLayer = LayerType.None;

    private BeatManager _beatManager = null;
    private FeedbackHandler _feedbackHandler = null;

    [SerializeField] private HitSparksFeedback _sparksFeedback = null;

    public DrumController Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _beatManager = ServiceLocator.Get<BeatManager>();

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _feedbackHandler.Initialize();

        return this;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (uint)_hammerLayer)
        {
            if (collision.gameObject.CompareTags("LHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.LTouch);
            }
            else if(collision.gameObject.CompareTags("RHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.RTouch);
            }

            _sparksFeedback.SetCollisionData(collision);
            _beatManager.DrumHit();
        }
    }
}