using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptimized : MonoBehaviour
{

    public Transform character;

    [Header("Camera Mechanics")]
    public float defaultAngleY;
    public float defaultAngleX;
    public float highClampAngle;
    public float lowClampAngle;
    public float inputSensitivity;

    [Header("Zoom Mechanics")]
    public float defaultDistance;
    public float maxDistance;
    public float minDistance;
    public float zoomSensitivity;

    [Header("Speed Camera")]
    public bool modifyFOV;
    public bool modifyDistance;
    public float fovSpeedModifier;
    public float distanceSpeedModifier;

    private float mouseY;
    private float mouseX;
    private float distance;

    // field of view based on player speed
    private Rigidbody playerRB;
    private float defaultFOV;
    private float finalDistance;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        distance = defaultDistance;
        mouseY = defaultAngleY;
        mouseX = defaultAngleX;

        playerRB = character.GetComponentInParent<Rigidbody>();
        defaultFOV = GetComponent<Camera>().fieldOfView;
    }

    void Update()
    {
        // setup rotation of sticks
        mouseX += Input.GetAxis("XBOX_Thumbstick_R_X") * inputSensitivity;
        mouseY -= Input.GetAxis("XBOX_Thumbstick_R_Y") * inputSensitivity;

        // setup rotation for mouse
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
        finalDistance = distance;
        SpeedCameraChange();

        // update camera's position and rotation
        Vector3 direction = new Vector3(0, 0, -finalDistance);
        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // apples all transformations to the camera
        transform.position = character.position + rotation * direction;
        transform.LookAt(character.position);
    }

    // modify camera values based on player's speed
    void SpeedCameraChange()
    {
        // take dot product of player's movement relative to camera direction to apply speed cam only in forward and backwards movement
        float vectorDot = Vector3.Dot(character.GetComponentInParent<PlayerMovement>().GetVelocity().normalized, transform.forward.normalized);

        // modifies camera distance
        if (modifyDistance)
        {
            finalDistance = distance + playerRB.velocity.magnitude * distanceSpeedModifier * Time.deltaTime * vectorDot;
        }
        // modifies camera field of view
        if (modifyFOV)
        {
            GetComponent<Camera>().fieldOfView = defaultFOV + (playerRB.velocity.magnitude * fovSpeedModifier) * vectorDot;
        }
    }
}
