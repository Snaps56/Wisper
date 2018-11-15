using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatOnWater : MonoBehaviour
{

    public float waterLevel = 0;
    public float floatHeight = 2.0f;
    public float waterDensity = 0.997f;
    public float downForce = 4.0f;

    private float forceFactor;
    private Vector3 floatForce;
    private Rigidbody rb;

    private bool touchingWater = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Water")
        {
            touchingWater = true;
        }
        else
        {
            touchingWater = false;
        }   
    }
    // Update is called once per frame
    void Update()
    {
        if(touchingWater == true)
        {
            Buoyancy();
        }
    }

    void Buoyancy()
    {
        forceFactor = 1.0f - ((transform.position.y - waterLevel) / floatHeight);

        if (forceFactor > 0)
        {
            floatForce = -Physics.gravity * rb.mass * (forceFactor - rb.velocity.y * waterDensity);
            floatForce += new Vector3(0, -downForce * rb.mass, 0);
            rb.AddForceAtPosition(floatForce, transform.position);
        }
    }
}
