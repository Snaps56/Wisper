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
        // check all detectable input controllers
        // toggles objects active based on whether player is using controller or keyboard input
        string[] names = Input.GetJoystickNames();
        //Loops through each element of the 'names array'
        for(int i = 0; i < names.Length; i++)
        {
            //if the element has anything in it, then the player is using a controller
            if (names[i].Length > 0)
            {
                isUsingController = true;
                controllerObject.SetActive(true);
                keyboardMouseObject.SetActive(false);
            }
            //Otherwise, the player is using a mouse
            else
            {
                controllerObject.SetActive(false);
                keyboardMouseObject.SetActive(true);
            }
        }

    }

    //Detects collisions
    void OnTriggerStay(Collider other)
    {
        //Dectects movement
        if (other.gameObject.CompareTag("TutorialMove"))
        {
            //If you move, the UI dissapears
            if (Input.GetButton("PC_Axis_MovementZ") || Input.GetButton("PC_Axis_MovementX") || Input.GetAxis("XBOX_Thumbstick_L_X") != 0 || Input.GetAxis("XBOX_Thumbstick_L_Y") != 0)
            {
                //Makes the UI dissapear
                other.gameObject.SetActive(false);
            }
        }
        //Detects looking
        else if (other.gameObject.CompareTag("TutorialLook"))
        {
            //If you look around, the Look UI dissapears
            if (Input.GetAxis("XBOX_Thumbstick_R_X") != 0 || Input.GetAxis("XBOX_Thumbstick_R_Y") != 0 || Input.GetAxis("PC_Mouse_X") != 0 || Input.GetAxis("PC_Mouse_Y") != 0)
            {
                //Makes the UI dissapear
                other.gameObject.SetActive(false);
            }
        }
        //If you rise, the Rise UI dissapears
        else if (other.gameObject.CompareTag("TutorialAscend"))
        {
            //If you press the rise buttons
            if (Input.GetButton("PC_Axis_MovementY") || Input.GetButton("XBOX_Axis_MovementY"))
            {
                //Makes the UI dissapear
                other.gameObject.SetActive(false);
            }

        }
        //If you lift or throw an oabject, the Lift UI Dissapears
        else if (other.gameObject.CompareTag("TutorialLift"))
        {
            //If you press the lift or throw buttons
            if (Input.GetButton("PC_Mouse_Click_L") || Input.GetButton("PC_Mouse_Click_R") || Input.GetAxis("XBOX_Trigger_L") > 0 || Input.GetAxis("XBOX_Trigger_R") > 0)
            {
                //Makes the UI dissapear
                other.gameObject.SetActive(false);
            }
        }
    }
}