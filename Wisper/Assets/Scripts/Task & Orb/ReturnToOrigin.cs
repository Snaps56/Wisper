using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToOrigin : MonoBehaviour {

    private Vector3 startPos;

    // Use this for initialization
    void Start () {
        startPos = this.transform.position;
	}
	
    //Return back to it's original position
    void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Border")) {
            this.transform.position = startPos;
            this.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
        }
    }

}
