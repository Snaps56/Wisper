using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : MonoBehaviour {

    private float RandomNum;
    public NPCMovement NPCMovements;
    private bool playedAnimation;
    private Animator animator;
    private int count;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playedAnimation = false;

    }
    // Update is called once per frame

    void OnTriggerEnter(Collider other)
    {
        //when touches the waypoints, roll a dice between 0 and 100
        if(other.gameObject.tag == "WayPoint")
        {
            RandomNum = Random.Range(0, 100);
            Debug.Log("random: " + RandomNum);
        }
        if (RandomNum > 60)
        {
            if (playedAnimation == false)
            {
                NPCMovements.move = false;
                animator.SetBool("isSearching", true);
                playedAnimation = true;
                StartCoroutine(Search());
            }
        }  
    }



    IEnumerator Search()
    {
        //Debug.Log("Search");
        
        if(playedAnimation == true)
        {
            yield return new WaitForSeconds(5.0f);
            //Debug.Log("isSearching");
            animator.SetBool("isSearching", false);
            NPCMovements.move = true;
            playedAnimation = false; 
        }
    }
}
