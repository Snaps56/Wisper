using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverFloat : MonoBehaviour
{
    public GameObject Water;
    public float waterLevel;
    public float floatHeight = 100;
    public float bounceDamp = 0.05f;
    public Vector3 UnderwaterGravity;
    public Vector3 BouyancyCenterOffset;
    public GameObject FloatingPoint;
    public float Mass;

    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift = -Physics.gravity;

    protected Rigidbody Rigidbody;
    protected BoxCollider CenterofMass;

    void Awake()
    {
        // Get Component
        Rigidbody = GetComponent<Rigidbody>();
        waterLevel = Water.GetComponent<Transform>().position.y;
        Mass = GetComponent<Rigidbody>().mass;
}
    // Update is called once per frame
    void Update()
    {
            bool floating = FloatingPoint.GetComponent<WaterCollider>().floating;
            actionPoint = transform.position + transform.TransformDirection(BouyancyCenterOffset);
            forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

            if (floating == true)
            {
            Debug.Log("Together We Made it AGAIN");
            Debug.Log("Weight: " + Mass);

            UnderwaterGravity.y = 9.8f * Mass;

            upLift = UnderwaterGravity * Mass * (forceFactor - Rigidbody.velocity.y * bounceDamp);
            Rigidbody.AddForceAtPosition(upLift, actionPoint);
            }
    }
}
