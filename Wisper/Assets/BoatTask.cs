using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTask : MonoBehaviour
{
    public List<GameObject> waypoints;
    int currentWP = 0;
    public GameObject waypoint01;

    //used components
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(waypoint01.position - transform.position, 0.5);
    }
}
