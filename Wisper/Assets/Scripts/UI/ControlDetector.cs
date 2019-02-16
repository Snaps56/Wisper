using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDetector : MonoBehaviour {

    public bool isUsingController;
    public bool allowAutoActivate;
    public GameObject controllerObject;
    public GameObject keyboardMouseObject;

    private GameObject currentActiveObject;

    // Use this for initialization
    void Start ()
    {
        controllerObject.SetActive(false);
        keyboardMouseObject.SetActive(true);
        currentActiveObject = keyboardMouseObject;
    }

    // Update is called once per frame
    void Update()
    {
        // check all detectable input controllers
        // toggles objects active based on whether player is using controller or keyboard input
        string[] names = Input.GetJoystickNames();
        //Loops through each element of the 'names array'

        //if the element has anything in it, then the player is using a controller
        //Otherwise, the player is using a mouse
        // Debug.Log("Using Controller: " + isUsingController);
        if (names[0] != "")
        {
            isUsingController = true;
            if (allowAutoActivate)
            {
                controllerObject.SetActive(true);
                keyboardMouseObject.SetActive(false);
            }
            currentActiveObject = controllerObject;
        }
        else
        {
            isUsingController = false;
            if (allowAutoActivate)
            {
                controllerObject.SetActive(false);
                keyboardMouseObject.SetActive(true);
            }
            currentActiveObject = keyboardMouseObject;
        }
        /*
        for (int i = 0; i < names.Length; i++)
        {
            Debug.Log(names[i]);
        }
        */
    }
    public GameObject GetCurrentActiveObject()
    {
        return currentActiveObject;
    }
}