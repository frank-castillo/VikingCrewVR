using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DrumController : MonoBehaviour
{
    [Header("Drums's Component")]
    private Animator drumAnimator;
    private AudioSource drumSource;

    [Header("Drums's Audio")]
    [SerializeField] private AudioClip weakHit;
    [SerializeField] private AudioClip strongHit;

    private void Awake()
    {
        drumAnimator = GetComponent<Animator>();
        drumSource = GetComponentInChildren<AudioSource>();
    }

    public void DrumWeakHit()
    {
        drumAnimator.SetTrigger("Drum Weak Hit");
        drumSource.PlayOneShot(weakHit);
    }

    public void DrumStrongHit()
    {
        drumAnimator.SetTrigger("Drum Strong Hit");
        drumSource.PlayOneShot(strongHit);
    }

    #region Editor

#if UNITY_EDITOR
    [CustomEditor(typeof(DrumController))]
    public class DrumControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrumController drumController = (DrumController)target;

            GUILayout.Space(10F);
            GUILayout.BeginHorizontal("Box");

            if (GUILayout.Button("Weak Hit"))
            {
                drumController.DrumWeakHit();
            }

            if (GUILayout.Button("Strong Hit"))
            {
                drumController.DrumStrongHit();
            }

            GUILayout.EndHorizontal();
        }
    }
#endif

    #endregion
}