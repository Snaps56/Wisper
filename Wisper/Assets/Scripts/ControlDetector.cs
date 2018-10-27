using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDetector : MonoBehaviour {

    public bool isUsingController;
    public GameObject controllerObject;
    public GameObject keyboardMouseObject;

    // Use this for initialization
    void Start ()
    {
        controllerObject.SetActive(false);
        keyboardMouseObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        string[] names = Input.GetJoystickNames();
        for(int i = 0; i < names.Length; i++)
        {
            if (names[i].Length > 0)
            {
                isUsingController = true;
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
}