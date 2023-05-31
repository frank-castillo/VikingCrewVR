using System;
using UnityEngine;

public abstract class ValueAsset<T> : ScriptableObject
{
    [SerializeField] protected T value;
    public Action Changed;

    public T GetValue { get => value; }

    public abstract void Add(T amount);

    public abstract void Substract(T amount);

    public void SetValue(T input)
    {
        value = input;
        Changed?.Invoke();
    }

    private void OnEnable()
    {
        Changed?.Invoke();
    }
}


   