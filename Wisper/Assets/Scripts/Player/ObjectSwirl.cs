using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwirl : MonoBehaviour {

	public bool isSwirling;

	// Use this for initialization
	void Start () {
		isSwirling = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			isSwirling = true;
		}
		if (isSwirling && Input.GetKeyUp (KeyCode.K)) {
			isSwirling = false;
		}
	}
}
