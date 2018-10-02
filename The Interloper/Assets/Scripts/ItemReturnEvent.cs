using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReturnEvent : MonoBehaviour
{

    public GameObject hat;
    public GameObject npc;
    private Rigidbody rb;

    private float npcHeadPos;
    private bool pick = false;
    Vector3 hatPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            pick = true;

        }
    }

    // Update is called once per frame
    void pickUp()
    {
        hatPos = new Vector3(npc.transform.position.x, npc.transform.position.y + npcHeadPos, npc.transform.position.z);
        hat.transform.position = hatPos;
        rb.isKinematic = true;
        hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;


    }

    private void Update()
    {

        if (pick == true)
        {
            pickUp();
        }
    }
}
