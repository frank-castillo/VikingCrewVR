using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        shipPosition = transform.position;
    }

    private void Update()
    {
        BuoyancyEffect();
    }

    private void BuoyancyEffect()
    {
        if (!enableBuoyancy) return;

        float newY = shipPosition.y + height * Mathf.Sin(Time.time * speedForY);
        float newX = shipPosition.x + width * Mathf.Sin(Time.time * speedForX);
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

}
