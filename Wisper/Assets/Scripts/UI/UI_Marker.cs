using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Marker : MonoBehaviour {
    
    public Camera mainCamera;
    public Canvas mainCanvas;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = mainCamera.WorldToScreenPoint(transform.parent.position);
	}
}
