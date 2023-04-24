using FeedbackSystem;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    // TODO: Move to CrewFeebackHandler,EnvironmentFeebackHandler, DrumFeebackHandler as in "UML" in Discord
    [SerializeField] private List<FeedbackPlayer> _crewFeedbackPlayers = null;
    [SerializeField] private List<FeedbackPlayer> _environmentFeedbackPlayers = null;
    [SerializeField] private List<FeedbackPlayer> _drumsFeedbackPlayers = null;

    public FeedbackManager Initialize()
    {
        Debug.Log($"<color=Lime> {this.GetType()} starting setup. </color>");

        return this;
    }

    // Example
    // TODO: Move to CrewFeebackHandler,EnvironmentFeebackHandler, DrumFeebackHandler as in "UML" in Discord
    public void PlayRandomCrewFeedback()
    {
        int randomIndex = Random.Range(0, _crewFeedbackPlayers.Count);
        _crewFeedbackPlayers[randomIndex].Play();
    }

    public void PlayRandomEnvironmentFeedback()
    {
        int randomIndex = Random.Range(0, _environmentFeedbackPlayers.Count);
        _environmentFeedbackPlayers[randomIndex].Play();
    }

    public void PlayRandomDrumFeedback()
    {
        int randomIndex = Random.Range(0, _drumsFeedbackPlayers.Count);
        _drumsFeedbackPlayers[randomIndex].Play();
    }
}