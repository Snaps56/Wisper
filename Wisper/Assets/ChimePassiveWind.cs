using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimePassiveWind : MonoBehaviour {

    private Vector3 windVector = new Vector3(0, 0, 0);
    private Rigidbody rb;
    public float maxStrength;
    private float passiveWindStrength;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        passiveWindStrength = Random.Range(-maxStrength, maxStrength);
        windVector = new Vector3(passiveWindStrength, 0, passiveWindStrength);
        rb.AddForce(windVector);
	}
}
