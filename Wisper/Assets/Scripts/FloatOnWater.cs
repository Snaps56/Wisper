using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class FloatOnWater : MonoBehaviour
{

    public float floatHeight = 0;
    public float waterDensity = 0;
    public float downForce = 0;

    private float forceFactor;
    private Vector3 floatForce;
    //private Rigidbody rb;

    private float waterLevel;

    public GameObject[] objects;
    public GameObject cubeTest;

    private bool touchingWater = false;

    void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("PickUp");
        waterLevel = transform.position.y;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {

            touchingWater = true;
        }
        else
        {
            touchingWater = false;
        }

        if (touchingWater == true)
        {
            Buoyancy(other.GetComponent<Rigidbody>());
        }
    }

    void Buoyancy(Rigidbody pickupable)
    {

        forceFactor = 1.0f - ((pickupable.transform.position.y - waterLevel) / floatHeight);
        forceFactor = 1.0f - ((pickupable.GetComponent<Rigidbody>().mass - waterLevel) / floatHeight);
        floatForce = -Physics.gravity * pickupable.GetComponent<Rigidbody>().mass * (forceFactor - pickupable.GetComponent<Rigidbody>().velocity.y * waterDensity);
        floatForce += new Vector3(0, -downForce * pickupable.GetComponent<Rigidbody>().mass, 0);
        //rb.AddForceAtPosition(floatForce, transform.position);
        pickupable.GetComponent<Rigidbody>().AddForceAtPosition(floatForce, pickupable.transform.position);
    }
}