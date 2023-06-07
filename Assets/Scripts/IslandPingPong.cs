using UnityEngine;

public class IslandPingPong : MonoBehaviour
{
    // Transforms to act as start and end markers for the journey.
    public Transform startMarker;
    public Transform endMarker;

    // Movement speed in units per second.
    public float speed = 1.0F;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    void Start()
    {
        // Keep a note of the time the movement started.
        startTime = Time.time;

        // Calculate the journey length.
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }

    // Move to the target end position.
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Use PingPong function to calculate the position between start and end points
        Vector3 newPosition = Vector3.Lerp(startMarker.position, endMarker.position, Mathf.PingPong(distCovered, 1f));

        // Set the object's position to the calculated position
        transform.position = newPosition;
    }
}
