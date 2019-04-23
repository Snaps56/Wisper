using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    /*****Purpose of this script *****/
    // This script was created to apply the physical changes that occur in the windmill
    //Such as disableing, enabling, and destroying the appropriate GameObjects

    public GameObject windimllSpinner;
    public GameObject brokenPart;
    public GameObject fixedWing;
    public GameObject wingCloth;
    private Rigidbody rb;
    private PersistantStateData persistantStateData;
    private bool attached = false;
    private bool updatedAttachCount = false;

    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
        if ((bool)persistantStateData.stateConditions["WindmillTaskDone"])
        {
            fixedWing.SetActive(true);
            wingCloth.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        //If a collision is made
        if (attached)
        {
            //Run the attached function
            Attached();
        }
	}
    //Update the windmill parts to be attached
    private void Attached()
    {
        //Debug.Log("ATTACHED");
        windimllSpinner.GetComponent<Windmill>().IncrementAttachCounter();
        fixedWing.SetActive(true);
        wingCloth.SetActive(true);
        this.gameObject.SetActive(false);
        attached = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Detected");
        //If the parts are touching, they are attached
        if (other.gameObject.name == brokenPart.gameObject.name)
        {
            Destroy(brokenPart);
            attached = true;
        }
    }
}
