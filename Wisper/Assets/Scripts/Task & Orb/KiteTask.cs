﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteTask : MonoBehaviour {

    // public variables
    public GameObject npc;

    // private varibles
    private bool getOrbs;
    private bool changed;


    private Vector3 finalPosition;
    private Vector3 kitePos;

    private float finalX;
    private float finalY;
    private float finalZ;

    private float rotSpeed;
    private Vector3 direction;

    private float x1;
    private float x2;
    private float z1;
    private float z2;
    private float maxHeight;

    private Animator animator;

    // Use this for initialization
    void Start () {
        //initialize varibles
        maxHeight = 234.47f;
        getOrbs = true;
        changed = false;

        x1 = -302.23f;
        x2 = -338.61f;
        z1 = -394.9f;
        z2 = -432.22f;

        //create an animator
        animator = GetComponent<Animator>();
        //animator.SetBool("Swaying", false);
        // finalX = -2.31f;
        // finalY = 43.0f;
        // finalZ = -84.21f;

        //direction = npc.transform.position - this.transform.position;
    }
	
    void FlyKite()
    {
       
        //check kite position within range
        if (transform.position.y >= maxHeight && transform.position.x <= x1 && transform.position.x >= x2 &&
            transform.position.z <= z1 && transform.position.z >= z2)
        {
            //have the kite stay in the air
            if(changed == false)
            {
                //set a final position for the kite
                finalPosition = new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z);
                changed = true;
            }
            transform.position = finalPosition;
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            
            if (getOrbs == true)
            {
                //get orbs for completing the task

                //animator.SetBool("Swaying", true);
                GetComponent<SpawnOrbs>().DropOrbs();
                getOrbs = false;
            }               
        }      
    }
    // Update is called once per frame
    void Update () {
        // have the kite and NPC face each other
        //direction = npc.transform.position - this.transform.position;
        /*
        if (transform.position.z > npc.transform.position.z)
        {
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        }
        else if (transform.position.z < npc.transform.position.z)
        {
            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        }
        */
        FlyKite();
	}
}
