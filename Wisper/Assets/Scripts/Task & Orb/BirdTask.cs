using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdTask : MonoBehaviour {

    
    public GameObject finalPlace;
    private Vector3 returned;

    private bool getOrbs;
    private bool pickedUp;
    private bool playAnimation;
    private float height;
    private Animator animator;
    private Vector3 jumpPos;
    private Vector3 startPos;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        pickedUp = false;
        getOrbs = true;
        playAnimation = true;
        height = 0.2f;
        animator.SetBool("Idle", false);
        jumpPos.y = 29.0f;
        startPos = new Vector3(-10.23f, 28.709f, 6.748087f);

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
        returned = new Vector3(finalPlace.transform.position.x +0.2f, finalPlace.transform.position.y + height, finalPlace.transform.position.z);
        transform.position = returned;
        
        if (transform.position == returned && getOrbs == true)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("FlapHop", false);

            GetComponent<SpawnOrbs>().DropOrbs();
            getOrbs = false;
            playAnimation = false;
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
}
