﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    private GameObject windimllSpinner;
    private GameObject brokenPart;
    private GameObject fixedWing;
    private Rigidbody rb;
    private PersistantStateData persistantStateData;
    private bool attached = false;
    private bool updatedAttachCount = false;
    private Windmill windmillScript;

    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
        windimllSpinner = GameObject.Find("RotationReset");
        if(this.gameObject.name.Contains("3"))
        {
            brokenPart = GameObject.Find("wing3_destroyed");
            fixedWing = GameObject.Find("wing_3");
        }
        else if (this.gameObject.name.Contains("5"))
        {
            brokenPart = GameObject.Find("wing5_destroyed");
            fixedWing = GameObject.Find("wing_5");
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //If the part is attached
        if (attached)
        {
            Attached();
        }
	}
    //Update the windmill parts to be attached
    private void Attached()
    {
        Debug.Log("ATTACHED");
        if (updatedAttachCount == false)
        {
            windimllSpinner.GetComponent<Windmill>().IncrementAttachCounter();
            updatedAttachCount = true;
        }

        fixedWing.SetActive(true);
        brokenPart.SetActive(false);
        attached = false;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected");
        //If the parts are touching, they are attached
        if (other.gameObject.name == brokenPart.gameObject.name)
        {
            attached = true;
        }
    }
}
