using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningLogo : MonoBehaviour {

    private float degPerSecond = -90;

	// Use this for initialization
	void Start () {
        degPerSecond *= Time.fixedDeltaTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Rotate(0, degPerSecond, 0);
	}
}
