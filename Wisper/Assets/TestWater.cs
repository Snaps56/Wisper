using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWater : MonoBehaviour {

    List<GameObject> buoyancyObjects = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // double check if the new lifted object is not the same as another object already in the array
        for (int i = 0; i < buoyancyObjects.Count; i++)
        {
            Vector3 positionInWater = buoyancyObjects[i].transform.position;
            positionInWater.y = transform.position.y + 0.75f;

            Vector3 deltaPosition = buoyancyObjects[i].transform.position - positionInWater;
            Debug.Log(deltaPosition);

            float buoyancyMult = 10f;
            Vector3 buoyancyForce = -deltaPosition * buoyancyMult;
            Rigidbody rb = buoyancyObjects[i].GetComponent<Rigidbody>();

            rb.AddForce(buoyancyForce * rb.mass);
            rb.AddForce(-rb.velocity * rb.mass);
            rb.AddTorque(-rb.angularVelocity * rb.mass * 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            // if pickupable object is touching water collider, add as a buoyant object
            if (buoyancyObjects.Count < 1)
            {
                buoyancyObjects.Add(other.gameObject);
            }
            else
            {
                // double check if the new buoyancy object is not the same as another object already in the array
                for (int i = 0; i < buoyancyObjects.Count; i++)
                {
                    if (buoyancyObjects.IndexOf(other.gameObject) == -1)
                    {
                        buoyancyObjects.Add(other.gameObject);
                    }
                }
            }
        }
    }
}
