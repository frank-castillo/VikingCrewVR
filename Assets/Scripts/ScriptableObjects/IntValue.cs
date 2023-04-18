using UnityEngine;

[CreateAssetMenu(fileName = "new int", menuName = "ScriptableObjects/ValueAssets/int")]
public class IntValue : ValueAsset<int>
{
    public override void Add(int amount)
    {
        value += amount;
        Changed?.Invoke();
    }

    public override void Substract(int amount)
    {
        value += amount;
        Changed?.Invoke();
    }
}