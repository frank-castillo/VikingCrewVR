using System.Collections;
using UnityEngine;

public class DrumEmitter : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float _preBeatPercentage = 0.9f;
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
    [SerializeField] private ParticleSystem _impactVFX = null;
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

        note.Activate(transform.position, _startingSize);

        StartCoroutine(TravelCoroutine(note));
    }

    private IEnumerator TravelCoroutine(NoteController note)
    {
        float timer = 0.0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = _destination.position;

        bool preBeatOccured = false;

        while (timer < _travelDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _travelDuration;
            note.transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            if (progress > _preBeatPercentage && preBeatOccured == false)
            {
                _beatManager.PreBeat();
                _beatManager.ActivateOnBeat();
                preBeatOccured = true;
            }

            yield return null;
        }

        note.EndTrail();
        _beatManager.Beat();
        _impactVFX.Play();

        StartCoroutine(WrapUpCoroutine(note));
    }

    private IEnumerator WrapUpCoroutine(NoteController note)
    {
        float timer = 0.0f;
        bool explosionOccured = false;
        bool particleActive = true;

        while (timer < _wrapUpDuration)
        {
            timer += Time.deltaTime;

            float progress = timer / _wrapUpDuration;

            float scaleProgress = progress * 2.0f;
            float scale = Mathf.Lerp(_startingSize, _endingSize, scaleProgress);

            if (particleActive)
            {
                if (scaleProgress > 1.0f)
                {
                    note.HideCore();
                    particleActive = false;
                }
                else
                {
                    note.ChangeScale(scale);
                }
            }

            if (progress > _explosionPercentage && explosionOccured == false)
            {
                _beatManager.EndOnBeat();
                explosionOccured = true;
            }

            yield return null;
        }

        note.Disable();
        _notePool.ReturnObject(note.gameObject);
    }
}
