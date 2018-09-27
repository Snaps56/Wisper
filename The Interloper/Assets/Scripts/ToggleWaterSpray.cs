using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWaterSpray : MonoBehaviour {

	private ParticleSystem WaterSpray;

	// Use this for initialization
	void Start () {
		WaterSpray = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.P)) {
			WaterSpray.Play ();
		} else {
			WaterSpray.Stop ();
		}
	}
}
