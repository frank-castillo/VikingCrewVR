using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioManifest")]
public class AudioManifest : ScriptableObject
{
    public List<AudioClipItem> AudioItems = new List<AudioClipItem>();
}
