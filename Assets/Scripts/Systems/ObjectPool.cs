using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _objectToPool = null;
    [SerializeField] private int _initialAmount = 5;
    private List<GameObject> _availableList = new List<GameObject>();
    private List<GameObject> _inUseList = new List<GameObject>();
    private int _totalPooledAmount = 0;

    public int TotalPooledAmount { get => _totalPooledAmount; }

    public void SetupPool()
    {
        LoadPool(_initialAmount);
    }

    private void LoadPool(int amount)
    {
        if (_objectToPool == null)
        {
            Debug.LogError($"Object to pool is empty! Please fix.");
            return;
        }

        for (int i = 0; i < amount; ++i)
        {
            GameObject gameObject = Instantiate(_objectToPool, transform);
            gameObject.SetActive(false);
            _availableList.Add(gameObject);
            ++_totalPooledAmount;
        }
    }

    public GameObject GetObject()
    {
        if (_availableList.Count == 0)
        {
            int amountToLoad = (int)(_totalPooledAmount * 0.5f);
            LoadPool(amountToLoad);
        }

        GameObject availableObject = _availableList[0];
        _availableList.Remove(availableObject);
        _inUseList.Add(_objectToPool);

        return availableObject;
    }

    public void ReturnObject(GameObject usedObject)
    {
        usedObject.SetActive(false);

        _inUseList.Remove(usedObject);
        _availableList.Add(usedObject);
    }
}
