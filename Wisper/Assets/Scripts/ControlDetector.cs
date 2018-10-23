using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDetector : MonoBehaviour {

    public bool isUsingController;
    public GameObject controllerObject;
    public GameObject keyboardMouseObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!isUsingController)
            {
                isUsingController = true;
            }
            else if (isUsingController)
            {
                isUsingController = false;
            }
        }

        if (isUsingController)
        {
            controllerObject.SetActive(true);
            keyboardMouseObject.SetActive(false);
        }
        else
        {
            controllerObject.SetActive(false);
            keyboardMouseObject.SetActive(true);
        }
	}
}
