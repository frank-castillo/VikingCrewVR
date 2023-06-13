using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationSFX : MonoBehaviour
{
    [SerializeField] private SFXType _sfxType = SFXType.None;
    private AudioManager _audioManager = null;

    public void PlayAnimationSoundEffect()
    {
        if(_audioManager == null)
        {
            _audioManager = ServiceLocator.Get<AudioManager>();
        }

        //_audioManager.PlaySFX(_sfxType);
    }
}
