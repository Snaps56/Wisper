using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonAnimations : MonoBehaviour
{
    private Animator animator;
    //public GameObject player;
    private float height;
    private bool walk;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), this.GetComponent<Collider>());
        walk = false;

        //height = 0.5f + gameObject.transform.position.y;
        
    }

    public bool GetWalk()
    {
        return walk;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "bigBoulder")
        {
            Debug.Log("Colliding");
            animator.SetBool("onBoulder", false);
        }
        /*
        else //(other.gameObject.tag == "Terrain")
        {
            animator.SetBool("onBoulder", true);
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "bigBoulder" || other.gameObject.tag == "Terrain")
        {
            //Debug.Log("Playing landed animation");
            animator.SetTrigger("Landed");
        }
        if (other.gameObject.name == "bigBoulder")
        {
            animator.SetBool("onBoulder", true);
            Debug.Log("onBoulder!!");
        }
        if (other.gameObject.tag == "Terrain" && other.gameObject.name != "bigBoulder")
        {
            animator.SetBool("onTerrain", true);

        }
    }

    

   

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("height: " + height);
       // Debug.Log("shellster: " + gameObject.transform.position.y);
        //Physics.IgnoreCollision(player.GetComponent<Collider>(), this.GetComponent<Collider>());
    }
}
