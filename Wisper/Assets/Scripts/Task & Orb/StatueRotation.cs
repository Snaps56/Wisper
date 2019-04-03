using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRotation : MonoBehaviour {

	public Transform rotatePoint;
	public Transform statueShift;

	// Use this for initialization
	void Start () {
		//transform.RotateAround (rotatePoint.position, Vector3.left, 30);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (statueShift.rotation.z);
		if (transform.rotation.z > .185f) {
			Debug.Log ("Too far +!");
			//var rot = transform.localRotation.eulerAngles;
			//rot.Set (0f, 0f, .185f);
			//transform.localRotation = Quaternion.Euler (rot);
			//transform.rotation = Quaternion.RotateTowards(transform.rotation, rotatePoint.rotation, 1 * Time.deltaTime);
			//transform.Rotate(0, 0, .19f);
		} else if (transform.rotation.z < -.185f) {
			Debug.Log ("Too far -!");
			//transform.Rotate(0, 0, -.19f);
		}
		//transform.RotateAround (rotatePoint.position, Vector3.right, 10 * Time.deltaTime);
	}
}
