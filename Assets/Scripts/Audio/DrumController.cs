using UnityEngine;

public class DrumController : MonoBehaviour
{
    [Header("Collider Layer Integer Reference")]
    [SerializeField] private int _hammerLayer = 8;

    private BeatManager _beatManager = null;
    private FeedbackHandler _feedbackHandler = null;

    [SerializeField] private HitSparksFeedback _sparksFeedback = null;

    public void Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        _beatManager = ServiceLocator.Get<BeatManager>();

        _feedbackHandler = GetComponent<FeedbackHandler>();
        _feedbackHandler.Initialize();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _hammerLayer)
        {
            if (collision.transform.CompareTag("LHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.LTouch);
            }
            else if(collision.transform.CompareTag("RHand"))
            {
                _beatManager.SetActiveController(OVRInput.Controller.RTouch);
            }

            _sparksFeedback.PlayHitSparks(collision);
            _beatManager.DrumHit();
        }
    }
}