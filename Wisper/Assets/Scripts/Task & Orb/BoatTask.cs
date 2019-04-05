using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTask : MonoBehaviour
{
    //public GameObject waypoint01;
    public Transform waypoint01;
    public Transform waypoint02;

    public float rotSpeed = 0.3f;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 direction;

    //used components
    private Rigidbody rb;
    private bool freeSailing = true;
    private bool waypointSwap = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WayPoint01")
        {
            waypointSwap = true;
            Debug.Log("Together We Made it");
        }
        if (other.name == "BoatTaskWall")
        {
            freeSailing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //find the vector pointing from our position to the target
        if (!waypointSwap)
        {
            direction = (waypoint01.position - transform.position).normalized;
        }

        if (waypointSwap)
        {
            direction = (waypoint02.position - transform.position).normalized;
        }

        //this.transform.rotation.x = 0;
        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(direction);
        if (!freeSailing)
        {
            //rotate us over time according to speed until we are in the required rotation
            // rotate only if boat is farther than 1 unit from waypoint
            if (!waypointSwap && Vector3.Distance(transform.position, waypoint01.position) > 10)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
            }
            else if (waypointSwap && Vector3.Distance(transform.position, waypoint02.position) > 10)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotSpeed);
            }
            if (!waypointSwap)//&& Vector3.Distance(transform.position, waypoint01.position))
            {
                //this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                rb.AddForce((waypoint01.transform.position - transform.position) * 4);
            }
            else if (waypointSwap)// && Vector3.Distance(transform.position, waypoint02.position))
            {
                rb.AddForce((waypoint02.transform.position - transform.position) * 4);
            }
        }
    }
}
