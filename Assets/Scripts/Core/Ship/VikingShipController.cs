using UnityEngine;

public class VikingShipController : MonoBehaviour
{
    [Header("Ship's Buoyancy Controllers")]
    [SerializeField] private bool enableBuoyancy = false;

    [Space]

    [SerializeField] [Range(0, 10)] private float height = 0.1F;
    [SerializeField] [Range(0, 10)] private float speedForY = 1F;

    [Space]

    [SerializeField] [Range(0, 10)] private float width = 0.05F;
    [SerializeField] [Range(0, 10)] private float speedForX = 2F;
    private Vector3 shipPosition;

    [Header("Ship's Sail Controllers")]
    [SerializeField] private bool enableSail = false;
    [SerializeField] private Cloth[] sailCloths;
    [SerializeField] private Vector3 sailDirection = new Vector3(0F, 0F, -20F);

    private void Start()
    {
        shipPosition = transform.position;
    }

    private void Update()
    {
        //BuoyancyEffect();
        //SailEffect();
    }

    private void BuoyancyEffect()
    {
        if (!enableBuoyancy) return;

        float newY = shipPosition.y + height * Mathf.Sin(Time.time * speedForY);
        float newX = shipPosition.x + width * Mathf.Sin(Time.time * speedForX);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    private void SailEffect()
    {
        if (enableSail)
        {
            foreach (var sail in sailCloths)
            {
                sail.externalAcceleration = sailDirection;
            }
        }
        else
        {
            foreach (var sail in sailCloths)
            {
                sail.externalAcceleration = Vector3.one;
            }
        }
    }

}
