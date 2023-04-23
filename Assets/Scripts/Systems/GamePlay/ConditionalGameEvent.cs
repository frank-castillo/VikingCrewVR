using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

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
            IConditional conditional = null;

            try
            {
                conditional = conditionalObject.GetComponent<IConditional>();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("An exception ({0}) occurred.", e.GetType().Name);
                Console.WriteLine("Message:\n   {0}\n", e.Message);
                return;
            }

            conditions.Add(conditional);
            conditional.OnComplete += CheckConditions;
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
