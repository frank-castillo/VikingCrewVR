using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NoteSet")]
public class NoteSet : ScriptableObject
{
    public List<BeatDirection> NoteOrder = new List<BeatDirection>();
}