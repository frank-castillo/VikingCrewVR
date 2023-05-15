using Ditzelgames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterFloat : MonoBehaviour
{
    //public properties
    public float AirDrag = 1;
    public float WaterDrag = 10;
    public bool AffectDirection = true;
    public bool AttachToSurface = false;
    public Transform[] FloatPoints;

    //used components
    protected Rigidbody rigidbody;
    protected Waves Waves;

    //water line
    protected float WaterLine;
    protected Vector3[] WaterLinePoints;

    //help Vectors
    protected Vector3 smoothVectorRotation;
    protected Vector3 targetUp;
    protected Vector3 centerOffset;

    public Vector3 Center { get { return transform.position + centerOffset; } }

    // Start is called before the first frame update
    void Awake()
    {
        //get components
        Waves = FindObjectOfType<Waves>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        //compute center
        WaterLinePoints = new Vector3[FloatPoints.Length];
        for (int i = 0; i < FloatPoints.Length; i++)
            WaterLinePoints[i] = FloatPoints[i].position;
        centerOffset = PhysicsHelper.GetCenter(WaterLinePoints) - transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Default water surface
        var newWaterLine = 0f;
        var pointUnderWater = false;

        // Set WaterLinePoints and WaterLine
        for (int i = 0; i < FloatPoints.Length; i++)
        {
            //height
            WaterLinePoints[i] = FloatPoints[i].position;
            WaterLinePoints[i].y = Waves.GetHeight(FloatPoints[i].position);
            newWaterLine += WaterLinePoints[i].y / FloatPoints.Length;
            if (WaterLinePoints[i].y > FloatPoints[i].position.y)
            {
                pointUnderWater = true;
            }
        }

        var waterLineDelta = newWaterLine - WaterLine;
        WaterLine = newWaterLine;

        // Compute up vector
        targetUp = PhysicsHelper.GetNormal(WaterLinePoints);

        // Gravity
        var gravity = Physics.gravity;
        rigidbody.drag = AirDrag;
        if (WaterLine > Center.y)
        {
            rigidbody.drag = WaterDrag;
            // Under water
            if (AttachToSurface)
            {
                // Attach to water surface
                rigidbody.position = new Vector3(rigidbody.position.x, WaterLine - centerOffset.y, rigidbody.position.z);
            }
            else
            {
                // Go up
                gravity = AffectDirection ? targetUp * -Physics.gravity.y : -Physics.gravity;
                transform.Translate(Vector3.up * waterLineDelta * 0.9f);
            }
        }
        rigidbody.AddForce(gravity * Mathf.Clamp(Mathf.Abs(WaterLine - Center.y),0,1));

        // Rotation
        if (pointUnderWater)
        {
            // Attach to water surface
            targetUp = Vector3.SmoothDamp(transform.up, targetUp, ref smoothVectorRotation, 0.2f);
            rigidbody.rotation = Quaternion.FromToRotation(transform.up, targetUp) * rigidbody.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (FloatPoints == null)
            return;

        for (int i = 0; i < FloatPoints.Length; i++)
        {
            if (FloatPoints[i] == null)
                continue;

            if (Waves != null)
            {

                //draw cube
                Gizmos.color = Color.red;
                Gizmos.DrawCube(WaterLinePoints[i], Vector3.one * 0.3f);
            }

            //draw sphere
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(FloatPoints[i].position, 0.1f);

        }

        //draw center
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(new Vector3(Center.x, WaterLine, Center.z), Vector3.one * 1f);
            Gizmos.DrawRay(new Vector3(Center.x, WaterLine, Center.z), targetUp * 1f);
        }
    }
}