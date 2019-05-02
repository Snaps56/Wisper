using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteTask : MonoBehaviour {

    // public variables
    public GameObject npc;
    public GameObject stringHolder;
    // private varibles
    private bool getOrbs;
    private bool changed;


    private Vector3 finalPosition;
    private Vector3 kitePos;
    private Vector3 stringHolderPos;

    private float finalX;
    private float finalY;
    private float finalZ;

    private float rotSpeed;
    private Vector3 direction;

    private float x1;
    private float x2;
    private float z1;
    private float z2;
    private float maxHeight;

    private Animator animator;

    // Use this for initialization
    void Start () {
        //initialize varibles
        maxHeight = 41.47f;
        getOrbs = true;
        changed = false;

        x1 = -176.89f;
        x2 = -213.27f;
        z1 = -423.75f;
        z2 = -461.22f;

        kitePos = transform.position;
        
        //create an animator
        animator = GetComponent<Animator>();

        //direction = npc.transform.position - this.transform.position;
    }
	
    void FlyKite()
    {
       
        //check kite position within range
        if (transform.position.y >= maxHeight && transform.position.x <= x1 && transform.position.x >= x2 &&
            transform.position.z <= z1 && transform.position.z >= z2)
        {
            //have the kite stay in the air
            if(changed == false)
            {
                //set a final position for the kite
                finalPosition = new Vector3(this.transform.position.x, this.transform.position.y , this.transform.position.z);
                changed = true;
            }
            //transform.position = finalPosition;
            //this.gameObject.GetComponent<Rigidbody>().useGravity = false;

            if (getOrbs == true)
            {
                //transform.position = finalPosition;
                GetComponent<SpawnOrbs>().DropOrbs();
                GetComponent<Rigidbody>().isKinematic = true;
                getOrbs = false;
                animator.enabled = true;
                stringHolder.transform.Rotate(0, -5.709f, -47.093f);
                stringHolder.transform.position = stringHolderPos;
                PersistantStateData.persistantStateData.ChangeStateConditions("KiteFlying", true);

            }               
        }      
    }
    // Update is called once per frame
    void Update () {

        stringHolderPos = new Vector3(stringHolder.transform.position.x - 0.113f, stringHolder.transform.position.y + 0.333f, stringHolder.transform.position.z + 0.25f);
        npc.transform.LookAt(transform.position);
        npc.transform.rotation = Quaternion.Euler(new Vector3(0, npc.transform.rotation.y * 180, 0));
        if (transform.position.y > kitePos.y)
        {
            transform.LookAt(npc.transform.position);
        }       
        FlyKite();
       // Debug.Log("kite Pos" + transform.position + " Rig" + GetComponent<Rigidbody>().transform.position);
	}
}
