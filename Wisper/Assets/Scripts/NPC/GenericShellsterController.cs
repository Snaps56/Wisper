using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericShellsterController : MonoBehaviour
{
    private Animator anim;
    public float chanceToFall = 0;
    private float time = 0;
    private float timeInterval = 1;
    private NPCMovement moveScript;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        moveScript = GetComponent<NPCMovement>();
        if(moveScript.move)
        {
            anim.SetBool("Walk", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > timeInterval)
        {
            Debug.Log("1 second has passed");
            time -= timeInterval;
            if (!anim.GetBool("GetUp") && !anim.GetBool("Fall"))
            {
                float fallCheck = Random.Range(0f, 1f);
                if (fallCheck < chanceToFall)
                {
                    Debug.Log("FALL: " + fallCheck);
                    anim.SetBool("Fall", true);
                }
                else
                {
                    Debug.Log("NO FALL: " + fallCheck);
                }
            }
                
            
        }

        if(anim.GetBool("Fall") || anim.GetBool("GetUp") || anim.GetBool("Idle"))
        {
            moveScript.move = false;
        }
        else
        {
            moveScript.move = true;
        }
    }
}
