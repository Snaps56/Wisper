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
    void Update () {
		if (attached == true)
        {
            Attached();
        }
	}

    private void Attached()
    {
        //rb.isKinematic = true;
        //rb.useGravity = false;
        //GetComponent<MeshCollider>().enabled = false;
        //GetComponent<Collider>().enabled = false;
        //transform.parent = windimll.transform;
        //transform.rotation = windimll.transform.rotation;

        //Debug.Log("ATTACHED");
        if (updatedAttachCount == false)
        {
            windimllSpinner.GetComponent<Windmill>().IncrementAttachCounter();
            updatedAttachCount = true;
        }

        fixedWing.SetActive(true);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Detected");
        if (other.gameObject.name == brokenPart.gameObject.name)
        {
            Destroy(brokenPart);
            attached = true;
        }
    }
}
