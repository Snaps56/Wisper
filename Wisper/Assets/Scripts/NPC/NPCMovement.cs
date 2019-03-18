using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> waypoints;
    int currentWP = 0;
    public float rotSpeed = 3.0f;
    public float speed = 0.5f;
    public float accuracyWP = 1.0f;
    
    public bool move = true;
    public bool loop = true;    // Loop movement around waypoints or stop at final waypoint (clears waypoints at end if false)

    // Use this for initialization
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {     


        Vector3 direction = player.transform.position - this.transform.position;
        
        //Movement loop
        if (waypoints.Count > 0)
        {
            //If move, transform npc toward waypoint (rot and pos with interpolation)
            if (move == true)
            {
                direction = waypoints[currentWP].transform.position - transform.position;
                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                this.transform.Translate(0, 0, Time.deltaTime * speed);
            }

            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
            {
                currentWP++;
				if (currentWP >= waypoints.Count) {
                    // If loop, next waypoint is 0. Else get rid of waypoints
                    if (loop)
                    {
                        currentWP = 0;
                    }
                    else
                    {
                        waypoints.Clear();
                    }
                    
                }
            }
        }

       
	}


}