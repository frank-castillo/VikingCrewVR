using UnityEngine;

public class RisingIsland : MonoBehaviour
{
    // Movement speed in units per second.
    [SerializeField] private float _risingSpeed = 25F;

    private Transform _emergePoint = null;

    // floats to act as start and end markers for the journey.
    private float _startY;
    private float _endY;

    // Time when the movement started.
    private float _startTime;

    // Total distance between the markers.
    private float _journeyLength;

    private bool _isOutOfWater = false;
    private bool _readyToEmerge = false;

    public bool ReadyToEmerge { set => _readyToEmerge = value; get => _readyToEmerge; }
    public float StartTime { set => _startTime = value; }

    public void Initialize(float startMarker, float endMarker, Transform emergePoint)
    {
        _startY = startMarker;
        _endY = endMarker;
        _emergePoint = emergePoint;

        // Keep a note of the time the movement started.
        _startTime = Time.time;

        // Calculate the journey length.
        _journeyLength = _journeyLength = Mathf.Abs(_endY - _startY);

        if (transform.position.z <= _emergePoint.position.z)
        {
            ResetInitialPosition();
        }
    }

    public void ResetInitialPosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = _startY;
        transform.position = newPosition;
        _isOutOfWater = false;
        _readyToEmerge = false;
    }

    public void EvaluateIfReadyToEmerge()
    {
        if (_readyToEmerge)
        {
            return;
        }

        if (transform.position.z >= _emergePoint.position.z)
        {
            _readyToEmerge = true;
            _startTime = Time.time;
        }
    }

    private void Update()
    {
        if (_isOutOfWater || !_readyToEmerge)
        {
            return;
        }

        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - _startTime) * _risingSpeed;

        // Calculate the normalized distance
        float normalizedDistance = distCovered / _journeyLength;
        normalizedDistance = Mathf.Clamp01(normalizedDistance);

        // Calculate the new y position using linear interpolation (lerp).
        float newY = Mathf.Lerp(_startY, _endY, normalizedDistance);

        // Set the object's position to the calculated position.
        Vector3 newPosition = transform.position;
        newPosition.y = newY;
        transform.position = newPosition;

        // Set the object's position to the calculated position
        transform.position = newPosition;

        if (normalizedDistance >= 1f)
        {
            _isOutOfWater = true;
        }
    }
}
