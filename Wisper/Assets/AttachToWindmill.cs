using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    public GameObject windimll;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
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
        rb.isKinematic = true;
        rb.useGravity = false;
        GetComponent<MeshCollider>().enabled = false;
        GetComponent<Collider>().enabled = false;
        transform.parent = windimll.transform;
        transform.position = windimll.transform.position + positionOffset;
        transform.rotation = windimll.transform.rotation;
        if(updatedAttachCount == false)
        {
            windimll.GetComponent<Windmill>().attachCount++;
            updatedAttachCount = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SpinningBase")
        {
            attached = true;
        }
    }
}
