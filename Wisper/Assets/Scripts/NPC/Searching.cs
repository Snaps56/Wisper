﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searching : MonoBehaviour {

    //create variables 
    private float RandomNum;
    public NPCMovement NPCMovements;
    private bool playedAnimation;
    private Animator animator;
    private int count;
    private void Start()
    {
        //create an animator
        animator = GetComponent<Animator>();
        playedAnimation = false;
        animator.SetBool("Idle", false);
    }
    // Update is called once per frame

        //create a trigger with waypoints tag
    void OnTriggerEnter(Collider other)
    {
        //when touches the waypoints, roll a dice between 0 and 100
        if(other.gameObject.tag == "WayPoint")
        {
            RandomNum = Random.Range(0, 100);
            count++;
            //Debug.Log(count + " random: " + RandomNum);
            
            // if random number is greater than 45 NPC will walk
            // else the NPC will stop and play its searching animation
            if (RandomNum > 45)
            {
                if (playedAnimation == false)
                {                   
                    animator.SetBool("Searching", true);                   
                    playedAnimation = true;
                    
                    if (playedAnimation == true)
                    {
                        NPCMovements.move = false;                       
                    }                                      
                }
                if (NPCMovements.move == false)
                {
                    StartCoroutine(Search());
                }
            }
        }    
    }

    IEnumerator Search()
    {
        //Debug.Log("Search");

        // have the NPC wait until the animation for 
        //searching is done playing before moving again
        if(playedAnimation == true)
        {
            yield return new WaitForSeconds(6.0f);
            //Debug.Log("isSearching");
            animator.SetBool("Searching", false);
            playedAnimation = false;
            if (playedAnimation == false)
            {
                NPCMovements.move = true;
            }
            else
            {
                NPCMovements.move = false;
            }
            //Debug.Log(NPCMovements.move);           
        }
    }
}
