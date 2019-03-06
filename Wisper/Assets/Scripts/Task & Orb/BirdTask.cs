using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTask : MonoBehaviour {

    
    public GameObject finalPlace;
    private Vector3 returned;
    
    private bool pickedUp;
    private bool playAnimation;
    private float height;
    private Animator animator;
    private Vector3 jumpPos;
    private Vector3 startPos;
    private PersistantStateData PSD;
    private bool spawnOrbs = true;
    // Use this for initialization
    void Start()
    {
        PSD = PersistantStateData.persistantStateData;
        animator = GetComponent<Animator>();
        pickedUp = false;
        
        playAnimation = true;
        height = 0.2f;
        animator.SetBool("Idle", false);

        if((bool)PSD.stateConditions["BirdInNest"])
        {
            spawnOrbs = false;
            SetBirdInTree();
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == finalPlace.tag)
        {
            pickedUp = true;   
        }
    }

    void PickUp()
    {
        SetBirdInTree();

        if (transform.position == returned)
        {
            PSD.ChangeStateConditions("BirdInNest", true);
            AnimateBirdInTree();
            if(spawnOrbs)
            {
                spawnOrbs = false;
                GetComponent<SpawnOrbs>().DropOrbs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (pickedUp == true)
        {
            PickUp();           
        }      
    }

    private void AnimateBirdInTree()
    {
        animator.SetBool("Idle", true);
        animator.SetBool("FlapHop", false);
        playAnimation = false;
    }

    private void SetBirdInTree()
    {
        returned = new Vector3(finalPlace.transform.position.x + 0.2f, finalPlace.transform.position.y + height, finalPlace.transform.position.z);
        transform.position = returned;
    }
}
