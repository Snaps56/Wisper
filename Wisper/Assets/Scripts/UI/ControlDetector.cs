using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDetector : MonoBehaviour {

    public bool isUsingController;
    public bool allowAutoActivate;
    public GameObject controllerObject;
    public GameObject keyboardMouseObject;

    private GameObject currentActiveObject;
    private bool emptyGameObjects;


    // Use this for initialization
    void Start ()
    {
        if (controllerObject == null && keyboardMouseObject == null)
        {
            emptyGameObjects = true;
        }
        else
        {
            controllerObject.SetActive(false);
            keyboardMouseObject.SetActive(true);
            currentActiveObject = keyboardMouseObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // check all detectable input controllers
        // toggles objects active based on whether player is using controller or keyboard input
        // Loops through each element of the 'names array'

        // if the element has anything in it, then the player is using a controller
        // Otherwise, the player is using a mouse
        string[] names = Input.GetJoystickNames();

        if (names.Length > 0)
        {
            if (names[0] == "")
            {
                isUsingController = false;

                if (!emptyGameObjects)
                {
                    if (allowAutoActivate)
                    {
                        controllerObject.SetActive(false);
                        keyboardMouseObject.SetActive(true);
                    }
                    currentActiveObject = keyboardMouseObject;
                }
            }
            else
            {
                isUsingController = true;

                if (!emptyGameObjects)
                {
                    if (allowAutoActivate)
                    {
                        controllerObject.SetActive(true);
                        keyboardMouseObject.SetActive(false);
                    }
                    currentActiveObject = controllerObject;
                }
            }
            // Debug.Log(names.Length + ", " + names[0]);
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
    public bool GetIsUsingController()
    {
        return isUsingController;
    }
}