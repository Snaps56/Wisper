using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTask : MonoBehaviour {

    //create variables
    public GameObject finalPlace;
    private Vector3 returned;
    
    private bool pickedUp;
    private bool playAnimation;
    private float height;
    private Animator animator;
    //private Vector3 jumpPos;
    //private Vector3 startPos;
    private PersistantStateData PSD;
    private bool spawnOrbs = true;
    // Use this for initialization
    void Start()
    {
        //initialize variables in the start function
        /// create an instance PSD
        PSD = PersistantStateData.persistantStateData;
        ///create an animator
        animator = GetComponent<Animator>();
        pickedUp = false;
        
        playAnimation = true;
        height = 0.2f;
        animator.SetBool("Idle", false);

        //while the condition for BirdInNest is false
        if((bool)PSD.stateConditions["BirdInNest"])
        {
            spawnOrbs = false;
            SetBirdInTree();
        }
    }

    //add a trigger to the bird
    private void OnTriggerStay(Collider col)
    {
        //when the bird is touching the nest
        if (col.gameObject.tag == finalPlace.tag)
        {
            pickedUp = true;   
        }
    }

    void PickUp()
    {
        SetBirdInTree();
        //W
        if (transform.position == returned)
        {
            //update the PSD and get orbs for compeleting the task
            PSD.ChangeStateConditions("BirdInNest", true);
            AnimateBirdInTree();
            if(spawnOrbs)
            {
                //get orbs once
                GetComponent<SpawnOrbs>().DropOrbs();
                spawnOrbs = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update when when the bird collides with the nest
        if (pickedUp == true)
        {
            PickUp();           
        }      
    }

    private void AnimateBirdInTree()
    {
        //change bird flapping animation to idle animation in the nest
        animator.SetBool("Idle", true);
        animator.SetBool("FlapHop", false);
        playAnimation = false;
    }

    private void SetBirdInTree()
    {
        // center the bird in the nest
        returned = new Vector3(finalPlace.transform.position.x + 0.2f, finalPlace.transform.position.y + height, finalPlace.transform.position.z);
        transform.position = returned;
    }
}
