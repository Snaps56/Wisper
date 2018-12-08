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

    float currentDistance;

    [Header("Zoom Mechanics")]
    public float defaultDistance;
    public float maxDistance;
    public float minDistance;
    public float zoomSensitivity;

    [Header("Speed Camera")]
    public bool modifyFOV;
    public float fovSpeedModifier;

    [Header("Collision")]
    public float sphereCastRadius;
    private float collisionDistance;
    private float linearInterpolate = 0f;
    private float linearInterpolateDiff = 1.5f;

    private float cameraY;
    private float cameraX;

    private bool cameraEnabled = true;

    private Vector3 direction;

    // field of view based on player speed
    private Rigidbody playerRB;
    private float defaultFOV;

    // Use this for initialization
    void Start()
    {
        // disable mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentDistance = defaultDistance;
        collisionDistance = defaultDistance;
        cameraY = defaultAngleY;
        cameraX = defaultAngleX;

        playerRB = character.GetComponentInParent<Rigidbody>();
        defaultFOV = GetComponent<Camera>().fieldOfView;
    }

    void Update()
    {
        // update camera values only when camera is enabled
        if (cameraEnabled)
        {
            // setup rotation of sticks
            cameraX += Input.GetAxis("XBOX_Thumbstick_R_X") * inputSensitivity;
            cameraY -= Input.GetAxis("XBOX_Thumbstick_R_Y") * inputSensitivity;

            // setup rotation for mouse
            cameraX += Input.GetAxis("PC_Mouse_X") * inputSensitivity;
            cameraY -= Input.GetAxis("PC_Mouse_Y") * inputSensitivity;

            cameraY = Mathf.Clamp(cameraY, lowClampAngle, highClampAngle);
        }
        
        /*
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
        */
        
        // Camera changing based on player speed
        SpeedCameraChange();

        // Camera collision
        CameraCollision();

        if (collisionDistance < defaultDistance)
        {
            if (linearInterpolate < 1)
            {
                linearInterpolate += linearInterpolateDiff * Time.deltaTime;
            }
            currentDistance = Mathf.Lerp(currentDistance, collisionDistance, linearInterpolate);
        }
        else
        {
            if (linearInterpolate > 0)
            {
                linearInterpolate -= linearInterpolateDiff * Time.deltaTime;
            }
            currentDistance = Mathf.Lerp(collisionDistance, currentDistance, linearInterpolate);
        }
        
        // update camera's position and rotation
        direction = -new Vector3(0, 0, currentDistance);

        Quaternion rotation = Quaternion.Euler(cameraY, cameraX, 0);

        // apples all transformations to the camera
        transform.position = character.position + rotation * direction;
        transform.LookAt(character.position);

    }

    // modify camera values based on player's speed
    void SpeedCameraChange()
    {
        // take dot product of player's movement relative to camera direction to apply speed cam only in forward and backwards movement
        float vectorDot = Vector3.Dot(character.GetComponentInParent<PlayerMovement>().GetVelocity().normalized, transform.forward.normalized);
        
        // modifies camera field of view
        if (modifyFOV)
        {
            GetComponent<Camera>().fieldOfView = defaultFOV + (playerRB.velocity.magnitude * fovSpeedModifier) * vectorDot;
        }
    }
    // camera collides with environment using sphere cast
    void CameraCollision()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - character.position;
        
        // check sphere cast
        if (Physics.SphereCast(character.position, sphereCastRadius, direction, out hit, defaultDistance))
        {
            // sphere cast only if the collider is an appropriate tag
            if (hit.collider.tag == "Water" || hit.collider.tag == "Terrain")
            {
                collisionDistance = (hit.point - character.position).magnitude;
            }
            // Debug.Log(hit.point);
        }
        else
        {
            collisionDistance = defaultDistance;
        }
    }
    public void DisableCameraMovement()
    {
        cameraEnabled = false;
    }
    public void EnableCameraMovement()
    {
        cameraEnabled = true;
    }
    public bool GetCameraEnabled()
    {
        return cameraEnabled;
    }
}
