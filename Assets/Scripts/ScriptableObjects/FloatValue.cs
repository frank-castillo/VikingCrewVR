using UnityEngine;

[CreateAssetMenu(fileName = "new float", menuName = "ScriptableObjects/ValueAssets/float")]
public class FloatValue : ValueAsset<float>
{
    public override void Add(float amount)
    {
        value += amount;
        Changed?.Invoke();
    }

    public override void Substract(float amount)
    {
        value -= amount;
        Changed?.Invoke();
    }
}