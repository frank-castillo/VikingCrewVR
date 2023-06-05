using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NoteSet")]
public class NoteCombo : ScriptableObject
{
    public List<BeatDirection> ComboList = new List<BeatDirection>();
}