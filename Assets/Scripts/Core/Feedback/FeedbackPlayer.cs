using System.Collections.Generic;
using UnityEngine;

namespace FeedbackSystem
{
    public class FeedbackPlayer : MonoBehaviour, IPlayable
    {
        [SerializeField] private List<Feedback> _feedbacks = null;

        // Potential Functionality
        // bool _isSequencedPlay
        // bool _isLooping
        // float looping time

        private void Awake()
        {
            foreach (var feedback in _feedbacks)
            {
                feedback.Initialize();
            }
        }

        public void Play()
        {
            foreach (IPlayable feedback in _feedbacks)
            {
                feedback.Play();
            }
        }
    }
}
