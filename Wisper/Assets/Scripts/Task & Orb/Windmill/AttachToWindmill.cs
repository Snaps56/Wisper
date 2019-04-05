using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToWindmill : MonoBehaviour {

    /*****Purpose of this script *****/
    // This script was created to reduce the amount of checks the destroyed windmill pieces
    // need to make to obtain the proper references

    private GameObject destroyedPart;
    // Use this for initialization
    void Start () {
        //Debug.Log("The name of the part: " + this.gameObject.name);
        if(this.gameObject.name.Contains("3"))
        {
            Debug.Log("Attaching the wing");
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
