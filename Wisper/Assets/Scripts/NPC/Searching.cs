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
            count++;
            Debug.Log(count + " random: " + RandomNum);
            
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
