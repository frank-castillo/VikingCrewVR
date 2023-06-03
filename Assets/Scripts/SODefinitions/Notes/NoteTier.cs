﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/NoteTiers")]
public class NoteTier : ScriptableObject
{
    public List<NoteCombo> NoteCombos = new List<NoteCombo>();
}
