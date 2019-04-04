using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTask : MonoBehaviour
{
    public List<GameObject> waypoints;
    int currentWP = 0;
    public GameObject waypoint01;

    //used components
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(waypoint01.transform.position - transform.position);
    }
}
