using System.Collections.Generic;
using UnityEngine;

public class HitSparksFeedback : Feedback
{
    [SerializeField] private List<ParticleSystem> _sparks = new List<ParticleSystem>();
    private Collision _collisionData = null;

    public void SetCollisionData(Collision collisionData) { _collisionData = collisionData; }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Play()
    {
        if (_collisionData == null)
        {
            return;
        }
        base.Play();
        PlayHitSparks();
    }

    public override void Stop()
    {
        if (!isPlaying)
        {
            return;
        }

        base.Stop();
    }

    public void PlayHitSparks()
    {
        int randomIndex = GetRandomSparksParticleIndex();
        if (randomIndex != -1)
        {
            Vector3 position = _collisionData.GetContact(0).point;
            ParticleSystem randomParticle = _sparks[randomIndex];
            randomParticle.transform.position = position;
            randomParticle.gameObject.SetActive(true);
            randomParticle.Play();

            _collisionData = null;
        }
    }

    private int GetRandomSparksParticleIndex()
    {
        int randomIndex = Random.Range(0, _sparks.Count);

        if (!_sparks[randomIndex].isPlaying)
        {
            return randomIndex;
        }
        else
        {
            for (int i = 0; i < _sparks.Count; i++)
            {
                if (!_sparks[i].isPlaying)
                {
                    return i;
                }
            }
        }

        return -1;
    }
}
