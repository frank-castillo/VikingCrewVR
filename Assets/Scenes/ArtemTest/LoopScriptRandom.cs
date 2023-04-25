using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace EpicToonFX
{
    public class LoopScriptRandom : MonoBehaviour
    {
        public List<GameObject> effects = null;
        private GameObject _chosenEffect = null;

        public float loopTimeLimit = 2.0f;

        [Header("Spawn without")]

        public bool disableLights = true;
        public bool disableSound = true;

        void Start()
        {
            PlayEffect();
        }

        public void PlayEffect()
        {
            StartCoroutine("EffectLoop");
        }

        IEnumerator EffectLoop()
        {
            _chosenEffect = PickRandom();
            GameObject effectPlayer = (GameObject)Instantiate(_chosenEffect, transform.position, transform.rotation);

            if (disableLights && effectPlayer.GetComponent<Light>())
            {
                effectPlayer.GetComponent<Light>().enabled = false;
            }

            if (disableSound && effectPlayer.GetComponent<AudioSource>())
            {
                effectPlayer.GetComponent<AudioSource>().enabled = false;
            }

            yield return new WaitForSeconds(loopTimeLimit);

            Destroy(effectPlayer);
            PlayEffect();
        }

        private GameObject PickRandom()
        {
            if (effects.Count == 0)
                return null;

            return effects[Random.Range(0, effects.Count)];
        }
    }
}