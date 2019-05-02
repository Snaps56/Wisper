using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchSitting : MonoBehaviour
{
    public bool rightSide;
    // Start is called before the first frame update
    void Start()
    {
        if(rightSide)
        {
            GetComponent<Animator>().SetBool("Right", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
