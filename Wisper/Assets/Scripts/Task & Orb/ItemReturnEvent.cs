using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReturnEvent : MonoBehaviour
{
    public GameObject npc;
    private Rigidbody rb;


    private bool pickedUp = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "NPC")
        {
            //StartCoroutine(TwoSecond());
            pickedUp = true;
        }
    }

    // Update is called once per frame
    void pickUp()
    {
        npc.transform.GetChild(0).gameObject.SetActive(true);
        npc.GetComponent<NPCMovement>().move = false;
        Destroy(this.gameObject);
        // x = -.31 z = .16f y = 1.25
        //hat.transform.position = npc.transform.position;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;

        

    }

    void Update()
    {

        if (pickedUp == true)
        {
            pickUp();
        }
    }

    //IEnumerator TwoSecond()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    pickedUp = true;
    //}
   
}
