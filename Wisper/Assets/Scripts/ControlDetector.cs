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

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("TutorialMove"))
        {
            if (Input.GetButton("PC_Axis_MovementZ") || Input.GetButton("PC_Axis_MovementX") || Input.GetAxis("XBOX_Thumbstick_L_X") != 0 || Input.GetAxis("XBOX_Thumbstick_L_Y") != 0)
            {
                //other.gameObject.GetComponent<SpriteRenderer>().color = new Color(19, 241, 25);
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("TutorialLook"))
        {
            if (Input.GetAxis("XBOX_Thumbstick_R_X") != 0 || Input.GetAxis("XBOX_Thumbstick_R_Y") != 0 || Input.GetAxis("PC_Mouse_X") != 0 || Input.GetAxis("PC_Mouse_Y") != 0)
            {
                other.gameObject.SetActive(false);
            }
        }
        else if (other.gameObject.CompareTag("TutorialAscend"))
        {
            if (Input.GetButton("PC_Axis_MovementY") || Input.GetButton("XBOX_Axis_MovementY"))
            {
                other.gameObject.SetActive(false);
            }

        }
        else if (other.gameObject.CompareTag("TutorialLift"))
        {
            if (Input.GetButton("PC_Mouse_Click_L") || Input.GetButton("PC_Mouse_Click_R") || Input.GetAxis("XBOX_Trigger_L") > 0 || Input.GetAxis("XBOX_Trigger_R") > 0)
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}