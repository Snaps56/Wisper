using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    public GameObject windimllSpinner;
    public GameObject brokenPart;
    public GameObject fixedWing;
    private Rigidbody rb;
    private PersistantStateData persistantStateData;
    private bool attached = false;
    private bool updatedAttachCount = false;

    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
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
        //Debug.Log("ATTACHED");
        if (updatedAttachCount == false)
        {
            windimllSpinner.GetComponent<Windmill>().IncrementAttachCounter();
            updatedAttachCount = true;
        }

        fixedWing.SetActive(true);
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
