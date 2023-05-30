using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NoteTiers")]
public class NoteSetTier : ScriptableObject
{
    public List<NoteSet> NoteSetList = new List<NoteSet>();
}
