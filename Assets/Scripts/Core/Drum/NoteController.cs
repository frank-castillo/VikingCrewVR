using UnityEngine;

public class NoteController : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer = null;
    [SerializeField] private ParticleSystem _coreVFX = null;

    public void Activate(Vector3 newPosition, float newScale)
    {
        _coreVFX.gameObject.SetActive(true);
        _trailRenderer.emitting = true;
        _trailRenderer.Clear();

        transform.position = newPosition;
        ChangeScale(newScale);

        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void HideCore()
    {
        _coreVFX.gameObject.SetActive(false);
    }

    public void EndTrail()
    {
        _trailRenderer.emitting = false;
    }

    public void ChangeScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
