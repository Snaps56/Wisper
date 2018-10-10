using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReturnEvent : MonoBehaviour
{

    public GameObject hat;
    public GameObject npc;
    private Rigidbody rb;

    public float npcHeadPosX = -0.31f;
    public float npcHeadPosY = 1.25f;
    public float npcHeadPosZ = 0.16f;
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
            StartCoroutine(TwoSecond());

        }
    }

    // Update is called once per frame
    void pickUp()
    {
        // x = -.31 z = .16f y = 1.25
        hatPos = new Vector3(npc.transform.position.x - npcHeadPosX, npc.transform.position.y + npcHeadPosY, npc.transform.position.z - npcHeadPosZ);
        hat.transform.position = hatPos;
        //hat.transform.position = npc.transform.position;
        rb.isKinematic = true;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;

        

    }

    void Update()
    {

        if (pick == true)
        {
            pickUp();
            GetComponent<HandleObjects>().enabled = false;
            hat.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    IEnumerator TwoSecond()
    {
        yield return new WaitForSeconds(2.0f);
        pick = true;
    }
   
}
