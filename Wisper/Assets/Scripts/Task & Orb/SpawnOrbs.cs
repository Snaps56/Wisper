﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOrbs : MonoBehaviour
{
    // Gameobjects required by Inspector
    public GameObject orb;
    public Transform player;

    // public variables for testing purposes
    public float orbCount; // number of orbs generated
    public float initialForceMultiplier; // force applied on orb on generation
    public float moveSpeedMultiplier; // how much faster orbs move toward the the player based on distance
    public float slowMultiplier; // how much friction reduces max speed of orbs after initial force
    public float orbSpawnOffsetY; // how high orbs spawn from the base position of parent object

    List<GameObject> orbsList = new List<GameObject>(); // a list of all active orbs in the scene
    
    // Update is called once per frame
    void Update()
    {
        // Debug button for dropping orbs
        if (Input.GetButtonDown("PC_Key_Orb"))
        {
            DropOrbs();
        }
        // make sure orbs list does not contain empty indices
        orbsList.TrimExcess();
    }

    private void FixedUpdate()
    {
        // for every orb generated
        for (int i = 0; i < orbsList.Count; i++)
        {
            // make sure index in array is not null
            if (orbsList[i] != null)
            {
                // obtain vector of orb and player
                Rigidbody rb = orbsList[i].GetComponent<Rigidbody>();
                Vector3 deltaPosition = player.position - orbsList[i].GetComponent<Transform>().position;
                deltaPosition = deltaPosition.normalized;

                // obtain vector of player and parent orb object to scale speed of orbs based on distance
                float distanceToBase = (player.position - transform.position).magnitude;
                Vector3 finalForce = deltaPosition * moveSpeedMultiplier * distanceToBase;

                // add a minimum speed to the orbs so they don't move too slow
                Vector3 minForce = deltaPosition * moveSpeedMultiplier * 10f;
                if (finalForce.magnitude < minForce.magnitude)
                {
                    finalForce = minForce;
                }

                // apply a pushing force to move orbs, but also a negative force to slow down orbs to cap the speed of orbs
                rb.AddForce(finalForce);
                rb.AddForce(-rb.velocity * slowMultiplier);
            }
        }
    }

    // drop orbs called only from outside class calls or via debug button
    public void DropOrbs (){

        // generate orbs based on the number of requested orbs to drop
		for (int i = 0; i < orbCount; i++) {

            // obtain player vector to make sure orbs spawn correctly
            Vector3 playerVector = player.transform.position - transform.position;
            Vector3 crossVector = Vector3.ProjectOnPlane(Random.insideUnitSphere, playerVector);
            Vector3 initialForce = crossVector.normalized;

            // make sure when generating orbs, that they only spawn upwards when applying initial force
            initialForce.y = Mathf.Abs(initialForce.y);
            initialForce = initialForce * initialForceMultiplier;

            // spawn orbs at position of parent object, along with y offset
            Vector3 orbSpawnPosition = transform.position;
            orbSpawnPosition.y += orbSpawnOffsetY;

            // instantiate orb and add it to the list of active orb objects
            GameObject orbInstance = Instantiate(orb, orbSpawnPosition, Quaternion.identity);
            Debug.Log("Made orb");
            orbsList.Add(orbInstance);

            orbInstance.GetComponent<Rigidbody>().AddForce(initialForce);
            // orbInstance.GetComponent<OrbSequence> ().setDestination (player, "player");
        }
	}
}
