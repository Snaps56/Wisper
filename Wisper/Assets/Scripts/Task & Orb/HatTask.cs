using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class HatTask : MonoBehaviour
{
    public GameObject npc;
    public Transform newParentObject;
    public Vector3 positionOffset;
    private Rigidbody rb;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    private PersistantStateData PSD;

    private bool makeHatFollow = false;


    private bool pickedUp = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if((bool)PSD.stateConditions["ShrineFirstConversationOver"])
        {
            if (col.gameObject.tag == "NPC")
            {
                //StartCoroutine(TwoSecond());
                pickedUp = true;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    void pickUp()
    {

        //npc.transform.Find("garden_hat").gameObject.SetActive(true);
        
        pickedUp = false;
        //npc.GetComponent<NPCMovement>().move = false;
        //npc.GetComponent<Animator>().SetBool("Idle", true);
        npc.GetComponent<SpawnOrbs>().DropOrbs();
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().stateConditions["ShamusHasHat"] = true;
        GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>().updateCount++;
        rb.isKinematic = true;
        rb.useGravity = false;
        GetComponent<Collider>().enabled = false;
        makeHatFollow = true;
        transform.parent = newParentObject;
        transform.position = newParentObject.position + positionOffset;
        transform.rotation = newParentObject.rotation;
        GamePad.SetVibration(playerIndex, 0f, 1f);
        StartCoroutine(Wait());
        //transform.tag = "Untagged";
        // x = -.31 z = .16f y = 1.25
        //hat.transform.position = npc.transform.position;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        //hat.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
        // npc.transform.GetChild(0).gameObject.SetActive(true);


    }

    void Update()
    {
        if (pickedUp == true)
        {
            pickUp();
        }

        if (PSD == null)
        {
            PSD = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        }
    }

    //IEnumerator TwoSecond()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    pickedUp = true;
    //}
   
}
