using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{

    public bool floating = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "RiverSegmentA")
        {
            //Debug.Log("Together We Made it");
            floating = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "RiverSegmentA")
        {
            floating = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
