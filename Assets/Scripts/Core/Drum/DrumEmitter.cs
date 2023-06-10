using System.Collections;
using UnityEngine;

public class DrumEmitter : MonoBehaviour
{
    [Header("Durations")]
    [SerializeField] private float _travelDuration = 0.0f;
    [SerializeField] private float _wrapUpDuration = 0.0f;

    [Header("Scale")]
    [SerializeField] private float _startingSize = 0.8f;
    [SerializeField] private float _endingSize = 0.0f;

    [Header("References")]
    [SerializeField] private Transform _destination = null;
    [SerializeField] private ObjectPool _notePool = null;

    public void Initialize()
    {
        _notePool.SetupPool();
    }

    public void ActivateParticle()
    {
        GameObject particle = _notePool.GetObject();
        particle.transform.position = transform.position;
        ChangeScale(particle.transform, _startingSize);

        particle.SetActive(true);

        StartCoroutine(TravelCoroutine(particle));
    }

    private IEnumerator TravelCoroutine(GameObject particle)
    {
        float timer = 0.0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = _destination.position;

        while (timer < _travelDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _travelDuration;
            particle.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            yield return null;
        }

        StartCoroutine(WrapUpCoroutine(particle));
    }

    private IEnumerator WrapUpCoroutine(GameObject particle)
    {
        float timer = 0.0f;
        while (timer < _wrapUpDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _wrapUpDuration;
            float scale = Mathf.Lerp(_startingSize, _endingSize, progress);
            ChangeScale(particle.transform, scale);

            yield return null;
        }

        particle.SetActive(false);
        _notePool.ReturnObject(particle);
    }

    private void ChangeScale(Transform newTransform, float scale)
    {
        newTransform.localScale = new Vector3(scale, scale, scale);
    }
}
