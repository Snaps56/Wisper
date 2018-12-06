using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_FacePlayer : MonoBehaviour {
    private Transform mainCamera;
	// Use this for initialization
	void Start () {
        mainCamera = GameObject.Find("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = mainCamera.rotation;
	}
}
