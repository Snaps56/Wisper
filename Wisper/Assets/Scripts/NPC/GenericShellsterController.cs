using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericShellsterController : MonoBehaviour
{
    public GenericShellsterMode mode;
    public float chanceToFall = 0;
    public bool regularLook = false;
    
    public bool lookAtWaypoint = false;
    public List<int> lookAtWaypoints = new List<int>();
    private bool triggerLook = false;
    private int prepLook = -1;

    private Animator anim;
    private float tripTime = 0;
    private float tripTimeInterval = 1;

    private float lookTime = 0;
    public float lookTimeInterval = 3;
    private NPCMovement moveScript;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        moveScript = GetComponent<NPCMovement>();
        if(mode.Equals(GenericShellsterMode.Walk))
        {
            if (moveScript.move)
            {
                anim.SetBool("Walk", true);
            }
        }
        else if(mode.Equals(GenericShellsterMode.Idle))
        {
            anim.SetBool("Idle",true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        tripTime += Time.deltaTime;
        lookTime += Time.deltaTime;

        if (tripTime > tripTimeInterval)
        {
            tripTime -= tripTimeInterval;
            //Debug.Log("1 second has passed");
            if (chanceToFall > 0 && !anim.GetBool("GetUp") && !anim.GetBool("Fall"))
            {
                float fallCheck = Random.Range(0f, 1f);
                if (fallCheck < chanceToFall)
                {
                    //Debug.Log("FALL: " + fallCheck);
                    anim.SetBool("Fall", true);
                }
                else
                {
                    //Debug.Log("NO FALL: " + fallCheck);
                }
            }
        }

        if(lookAtWaypoint)
        {
            if(prepLook >= 0)
            {
                if(moveScript.getCurrentWP() != prepLook)
                {
                    Debug.Log("Triggered shellster look at waypoint");
                    moveScript.move = false;
                    anim.SetBool("Look", true);
                    prepLook = -1;
                }
            }
            else
            {
                foreach(int wpIndex in lookAtWaypoints)
                {
                    if(wpIndex == moveScript.getCurrentWP())
                    {
                        prepLook = moveScript.getCurrentWP();
                    }
                }
            }
        }

        if(lookTime > lookTimeInterval)
        {
            lookTime -= lookTimeInterval;
            if(regularLook)
            {
                if(mode.Equals(GenericShellsterMode.Walk))
                {
                    moveScript.move = false;
                }
                anim.SetBool("Look", true);
            }
        }

        if(anim.GetBool("Walk") && !(anim.GetBool("Fall") || anim.GetBool("GetUp") || anim.GetBool("Look")))
        {
            moveScript.move = true;
        }
        else
        {
            moveScript.move = false;
        }
    }
}

public enum GenericShellsterMode {Walk, Idle};
