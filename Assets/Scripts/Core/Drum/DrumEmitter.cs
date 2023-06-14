using System.Collections;
using UnityEngine;

public class DrumEmitter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _preBeatPercentage = 0.9f;
    [SerializeField] private float _onBeatPercentage = 0.95f;
    [SerializeField] private float _explosionPercentage = 0.9f;

    [Header("Durations")]
    [SerializeField] private float _travelDuration = 0.0f;
    [SerializeField] private float _wrapUpDuration = 0.0f;

    [Header("Scale")]
    [SerializeField] private float _startingSize = 0.8f;
    [SerializeField] private float _endingSize = 0.0f;

    [Header("References")]
    [SerializeField] private Transform _destination = null;
    [SerializeField] private ObjectPool _notePool = null;
    private BeatManager _beatManager = null;

    public void Initialize()
    {
        _beatManager = ServiceLocator.Get<BeatManager>();

        _notePool.SetupPool();
    }

    public void ActivateParticle()
    {
        GameObject particle = _notePool.GetObject();
        NoteController note = particle.GetComponent<NoteController>();

        note.Reset();
        note.transform.position = transform.position;
        ChangeScale(note.transform, _startingSize);
        particle.SetActive(true);

        StartCoroutine(TravelCoroutine(note));
    }

    private IEnumerator TravelCoroutine(NoteController note)
    {
        float timer = 0.0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = _destination.position;

        bool preBeatOccured = false;
        bool onBeatOccured = false;

        while (timer < _travelDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _travelDuration;
            note.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            if (progress > _preBeatPercentage && preBeatOccured == false)
            {
                _beatManager.PreBeat();
                preBeatOccured = true;
            }
            if (progress > _onBeatPercentage && onBeatOccured == false)
            {
                _beatManager.ActivateOnBeat();
                onBeatOccured = true;
            }

            yield return null;
        }

        _beatManager.Beat();
        note.End();

        StartCoroutine(WrapUpCoroutine(note));
    }

    private IEnumerator WrapUpCoroutine(NoteController note)
    {
        float timer = 0.0f;
        bool explosionOccured = false;

        while (timer < _wrapUpDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _wrapUpDuration;
            float scale = Mathf.Lerp(_startingSize, _endingSize, progress);
            ChangeScale(note.transform, scale);

            if (progress > _explosionPercentage && explosionOccured == false)
            {
                _beatManager.EndOnBeat();
                explosionOccured = true;
            }

            yield return null;
        }

        note.gameObject.SetActive(false);

        _notePool.ReturnObject(note.gameObject);
    }

    private void ChangeScale(Transform newTransform, float scale)
    {
        newTransform.localScale = new Vector3(scale, scale, scale);
    }
}
