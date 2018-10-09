using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{

    public GameObject player;
    public GameObject hat;
    public GameObject npc;
    public GameObject[] waypoints;
    int currentWP = 0;
    public float rotSpeed = 3.0f;
    public float speed = 0.5f;
    public float accuracyWP = 1.0f;
    public float detection = 10.0f;
    public float stopLength = 5.0f;

    private bool move = true;
    Vector3 npcPos;
    Vector3 hatPos;
    //NavMeshAgent agent;

    // Use this for initialization
    void Start()
    {
         
    }
    // Update is called once per frame
    void Update()
    {
        waypoints[5].transform.position = hat.transform.position;
        Vector3 direction = player.transform.position - this.transform.position;
        Vector3 distanceFromHat = npc.transform.position - hat.transform.position;

        if (waypoints.Length > 0)
        {
            if (Vector3.Distance(waypoints[currentWP].transform.position, transform.position) < accuracyWP)
            {
                currentWP++;
                
                if (currentWP >= waypoints.Length - 1)
                {
                    
                    currentWP = 0;
                    
                }
               if ((hat != null && Vector3.Distance(transform.position, hat.transform.position) < detection))
                {
                    currentWP = 5;
                    
                }

            }
            if (currentWP == 5 && distanceFromHat.magnitude <= stopLength)
            {
                move = false;
                direction = waypoints[currentWP].transform.position - transform.position;
                this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
                waypoints[5].SetActive(false);
                
            }
        }


        if (move == true)
        {
            direction = waypoints[currentWP].transform.position - transform.position;
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, Time.deltaTime * speed);
        }
        

        
    }



}

