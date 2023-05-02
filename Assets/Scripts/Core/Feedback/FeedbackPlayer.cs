using System.Collections.Generic;
using UnityEngine;

namespace FeedbackSystem
{
    public class FeedbackPlayer : MonoBehaviour
    {
        [SerializeField] private List<Feedback> _feedbacks = null;

        private void Awake()
        {
            foreach (var feedback in _feedbacks)
            {
                feedback.Initialize();
            }
        }

        public void Play()
        {
            foreach (Feedback feedback in _feedbacks)
            {
                feedback.Play();
            }
        }

        public void Stop()
        {
            foreach (Feedback feedback in _feedbacks)
            {
                feedback.Stop();
            }
        }
    }
}
