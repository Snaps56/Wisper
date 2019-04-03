using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    private GameObject destroyedPart;
    // Use this for initialization
    void Start () {
        if(this.gameObject.name.Contains("3"))
        {
            destroyedPart = GameObject.Find("wing3_destroyed");
            destroyedPart.GetComponent<AttachToWindmill3>().brokenPart = this.gameObject;
        }
        else if (this.gameObject.name.Contains("5"))
        {
            destroyedPart = GameObject.Find("wing5_destroyed");
            destroyedPart.GetComponent<AttachToWindmill5>().brokenPart = this.gameObject;
        }
    }

}
