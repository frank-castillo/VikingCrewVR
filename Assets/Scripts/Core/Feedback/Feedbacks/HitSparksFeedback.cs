using System.Collections.Generic;
using UnityEngine;

public class HitSparksFeedback : Feedback
{
    [SerializeField] private List<ParticleSystem> _sparks = new List<ParticleSystem>();

    public override void Initialize()
    {
        base.Initialize();

    }

    public override void Play()
    {
        if (isPlaying)
        {
            return;
        }
        base.Play();
    }

    public override void Stop()
    {
        if (!isPlaying)
        {
            return;
        }

        base.Stop();
    }

    public void PlayHitSparks(Collision collisionData)
    {
        int randomIndex = GetRandomSparksParticleIndex();
        if (randomIndex != -1)
        {
            Vector3 position = collisionData.GetContact(0).point;
            ParticleSystem randomParticle = _sparks[randomIndex];
            randomParticle.transform.position = position;
            randomParticle.gameObject.SetActive(true);
            randomParticle.Play();
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
