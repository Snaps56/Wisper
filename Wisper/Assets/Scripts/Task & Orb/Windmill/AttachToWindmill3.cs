using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill3 : MonoBehaviour {

    public GameObject windimllSpinner;
    private GameObject brokenPart;
    private GameObject tagHelper;
    public GameObject fixedWing;
    private Rigidbody rb;
    private PersistantStateData persistantStateData;
    private bool attached = false;
    private bool updatedAttachCount = false;
    private Windmill windmillScript;

    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update ()
    {
        try
        {
            tagHelper = GameObject.FindGameObjectWithTag("WindmillPart3");
            brokenPart = tagHelper.transform.parent.gameObject;
        }
        catch (System.Exception e)
        {
            Debug.Log("Broken");
        }
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
            Debug.Log("Attached Value: " + attached);
        }
    }
}
