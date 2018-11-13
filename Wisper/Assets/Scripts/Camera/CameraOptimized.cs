using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptimized : MonoBehaviour
{

    public Transform character;

    [Header("Camera Mechanics")]
    public float highClampAngle;
    public float lowClampAngle;
    public float inputSensitivity;

    [Header("Zoom Mechanics")]
    public float defaultDistance;
    public float maxDistance;
    public float minDistance;
    public float zoomSensitivity;

    private float mouseY;
    private float mouseX;
    private float distance;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        distance = defaultDistance;
}

    void Update()
    {
        // setup rotation of sticks
        mouseX += Input.GetAxis("XBOX_Thumbstick_R_X") * inputSensitivity;
        mouseY -= Input.GetAxis("XBOX_Thumbstick_R_Y") * inputSensitivity;

        mouseX += Input.GetAxis("PC_Mouse_X") * inputSensitivity;
        mouseY -= Input.GetAxis("PC_Mouse_Y") * inputSensitivity;

        mouseY = Mathf.Clamp(mouseY, lowClampAngle, highClampAngle);
        
        // allow camera zoom via mouse scroll, but only within boundaries
        if (distance < maxDistance)
        {
            if (Input.GetAxis("PC_Mouse_Scroll") > 0)
            {
                distance += Input.GetAxis("PC_Mouse_Scroll") * zoomSensitivity;
            }
        }
        if (distance > minDistance)
        {
            if (Input.GetAxis("PC_Mouse_Scroll") < 0)
            {
                distance += Input.GetAxis("PC_Mouse_Scroll") * zoomSensitivity;
            }
        }

        // update camera's position and rotation
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        transform.position = character.position + rotation * direction;
        transform.LookAt(character.position);

    }
}
