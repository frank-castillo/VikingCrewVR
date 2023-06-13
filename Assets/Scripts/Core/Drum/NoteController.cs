using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRendererInner = null;
    [SerializeField] private TrailRenderer _trailRendererOuter = null;

    public void Reset()
    {
        _trailRendererInner.emitting = true;
        _trailRendererInner.Clear();

        _trailRendererOuter.emitting = true;
        _trailRendererOuter.Clear();
    }

    public void End()
    {
        _trailRendererInner.emitting = false;
        _trailRendererOuter.emitting = false;
    }
}
