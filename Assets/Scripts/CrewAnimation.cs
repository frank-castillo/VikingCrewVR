using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrewState
{
    emptyState,

    setIdleState,
    setRowState,

    idleState,
}

public class CrewAnimation : MonoBehaviour
{
    // Sorry about my bad programming skills... TTwTT

    [Header("Crew States")]
    [SerializeField] private CrewState currentState;

    [Header("Crew Component")]
    [SerializeField] private Animator[] crewAnimators;

    [Header("Crew Controllers")]
    [SerializeField] private Vector2 randomDelay = new Vector2(15F, 30F);
    private float nextActionTime = 0F;
    private float delay = 0F;

    private IEnumerator Start()
    {
        foreach (var animator in crewAnimators)
        {
            yield return new WaitForSeconds(Random.Range(0F, 0.15F));

            animator.SetTrigger("Idle");
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case CrewState.setIdleState:
                SetState("Idle");
                currentState = CrewState.idleState;
                break;

            case CrewState.setRowState:
                SetState("Row");
                break;

            case CrewState.idleState:
                Idle();
                break;
        }
    }

    public void Idle()
    {
        if (Time.time > nextActionTime)
        {
            int randomCrew = Random.Range(0, crewAnimators.Length);
            int randomAnimation = Random.Range(0, 3);

            if (randomAnimation == 1)
            {
                crewAnimators[randomCrew].SetTrigger("Arm Stretch");
            }
            else
            {
                crewAnimators[randomCrew].SetTrigger("Yawn");
            }

            delay = Random.Range(randomDelay.x, randomDelay.y);
            nextActionTime = Time.time + delay;
        }
    }

    public void SetState(string name)
    {
        currentState = CrewState.emptyState;

        foreach (var animator in crewAnimators)
        {
            animator.SetTrigger(name);
        }
    }

}
