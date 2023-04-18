using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConditionalGameEvent : MonoBehaviour
{
    private List<IConditional> conditions = new List<IConditional>();
    [SerializeField] private List<GameObject> conditionalObjects = new List<GameObject>();

    public UnityEvent OnCompleteGameEvent;

    private void Awake()
    {
        Initialize();
    }

    private void OnDestroy()
    {
        foreach (var condition in conditions)
        {
            condition.OnComplete -= CheckConditions;
        }
    }

    private void Initialize()
    {
        foreach (var conditionalObject in conditionalObjects)
        {
            if (conditionalObject.TryGetComponent<IConditional>(out IConditional conditional))
            {
                conditions.Add(conditional);
                conditional.OnComplete += CheckConditions;
            }
        }
    }

    public void CheckConditions()
    {
        bool isFailed = false;
        foreach (var condition in conditions)
        {
            if (condition.IsTrue() == false)
            {
                isFailed = true;
            }
        }

        if (isFailed)
        {
            return;
        }
        else
        {
            OnCompleteGameEvent?.Invoke();
        }
    }
}
