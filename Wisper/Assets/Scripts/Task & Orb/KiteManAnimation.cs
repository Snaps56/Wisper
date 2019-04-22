using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiteManAnimation : MonoBehaviour
{

    public GameObject kite;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Kite Pos" + kite.transform.position.y);
        if(kite.transform.position.y > 230)
        {
            animator.SetBool("KiteInAir", true);
        }
    }
}
