using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class HatTask : MonoBehaviour
{
    public GameObject npc;
    public Transform newParentObject;
    public Vector3 positionOffset;
    public GameObject theHat;
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
        PSD = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
        if ((bool)PSD.stateConditions["ShamusHasHat"])
        {
            PutTheGoshDarnHatOnTheGoshDarnShamus();
            Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if((bool)PSD.stateConditions["ShrineFirstConversationOver"])
        {
            if (col.gameObject.tag == "NPC" && col.gameObject == npc)
            {
                //StartCoroutine(TwoSecond());
                if(!(bool)PSD.stateConditions["ShamusHasHat"])
                {
                    pickedUp = true;
                }
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
        Destroy(gameObject);
    }

    void pickUp()
    {
        
        pickedUp = false;
        npc.GetComponent<SpawnOrbs>().DropOrbs();
        PSD.ChangeStateConditions("ShamusHasHat", true);
        PutTheGoshDarnHatOnTheGoshDarnShamus();
        GamePad.SetVibration(playerIndex, 0f, 1f);
        StartCoroutine(Wait());
    }

    private void PutTheGoshDarnHatOnTheGoshDarnShamus()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        //GetComponent<Collider>().enabled = false;
        //makeHatFollow = true;
        //transform.parent = newParentObject;
        //transform.position = newParentObject.position + positionOffset;
        //transform.rotation = newParentObject.rotation;
        theHat.SetActive(true);
        Destroy(this);
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
