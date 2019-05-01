using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAnimations : MonoBehaviour
{
    private bool playedAnimation;
    private Animator animator;
    public BigBoulderTask bigBoulderTask;
    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (bigBoulderTask.GetGrounded() == true && bigBoulderTask.GetWalk() == true)
        {
            animator.SetBool("Walk",true);
            animator.SetBool("Idle", false);
        }
        else
        {
            animator.SetBool("Idle", true);
        }

        Debug.Log("Grounded " + bigBoulderTask.GetGrounded());
        Debug.Log("Walk " + bigBoulderTask.GetWalk());
    }
}
