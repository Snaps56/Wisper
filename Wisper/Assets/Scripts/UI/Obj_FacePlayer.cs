using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj_FacePlayer : MonoBehaviour {
    private Transform mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = PlayerPersistance.player.transform.Find("Main Camera").gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = mainCamera.rotation;
	}
}
