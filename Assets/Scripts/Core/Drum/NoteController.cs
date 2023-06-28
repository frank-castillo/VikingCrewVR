using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer = null;

    public void Reset()
    {
        _trailRenderer.emitting = true;
        _trailRenderer.Clear();
    }

    public void End()
    {
        _trailRenderer.emitting = false;
    }
}
