using UnityEngine;

public class IslandMaterialSwapper : MonoBehaviour
{
    [SerializeField] private Material _transparentMaterial = null;
    [SerializeField] private Material _opaqueMaterial = null;
    private float _swapDistance = 2500f;

    private Renderer _renderer = null;
    private Camera _camera = null;
    private bool _isTransparent = true;
    private bool _initialized = false;

    public void MakeTransparent()
    {
        _isTransparent = true;
        _renderer.material = _transparentMaterial;
    }

    public void MakeOpaque()
    {
        _isTransparent = false;
        _renderer.material = _opaqueMaterial;
    }

    public void Initialize()
    {
        _camera = ServiceLocator.Get<CameraUtil>().Camera;
        _renderer = GetComponent<Renderer>();

        MakeTransparent();

        _initialized = true;
    }

    private void Update()
    {
        if (_initialized == false)
        {
            return;
        }

        TransparentUpdate();
    }

    private void TransparentUpdate()
    {
        if (_isTransparent == false)
        {
            return;
        }

        float distance = Mathf.Abs(transform.position.z - _camera.transform.position.z);
        EvaluateDistance(distance);
    }

    private void EvaluateDistance(float distance)
    {
        if (distance < _swapDistance)
        {
            MakeOpaque();
        }
    }
}
