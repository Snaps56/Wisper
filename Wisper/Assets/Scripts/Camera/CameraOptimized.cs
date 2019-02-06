using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptimized : MonoBehaviour
{

    public Transform focusPoint;
    public GameObject playerObject;

    [Header("Camera Mechanics")]
    public float defaultAngleY = 20;
    public float defaultAngleX = 100;
    public float highClampAngle = 80;
    public float lowClampAngle = -80;
    public float inputSensitivity = 2;

    float currentDistance;

    [Header("Smooth Cam")]
    public bool enableSmoothCam = true;
    public float smoothCamDecay = 0.9f;
    public float smoothCamRound = 0.04f;
    public float smoothSensitivityMultiplier = 0.04f;

    [Header("Zoom Mechanics")]
    public float defaultDistance = 4;
    public float maxDistance = 10;
    public float minDistance = 4;
    public float zoomSensitivity = 3;

    [Header("Speed Camera")]
    public bool modifyFOV = true;
    public float fovSpeedModifier = 0.5f;

    [Header("Adaptive Camera")]
    public float adaptiveStrength = 0.12f;
    public float adaptiveDelay = 3;
    public bool allowAdaptiveCam = false;
    
    private float initAdaptiveTimer;
    private float currentAdaptiveTimer;
    private bool playerNotMovingCam = false;
    private bool doAdaptiveCam = false;

    [Header("Collision")]
    public float sphereCastRadius = 1;
    private float collisionDistance;
    private float linearInterpolate = 0f;
    private float linearInterpolateDiff = 1.5f;

    private float cameraY;
    private float cameraX;

    private bool cameraEnabled = true;

    private Vector3 direction;
    
    private Vector3 deltaVector;

    // field of view based on player speed
    private Rigidbody playerRB;
    private float defaultFOV;
    float sideCameraScroll = 0;

    private float inputYAxis;
    private float inputXAxis;

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

        playerRB = focusPoint.GetComponentInParent<Rigidbody>();
        defaultFOV = GetComponent<Camera>().fieldOfView;
    }
    void Update()
    {
        // update camera values only when camera is enabled
        if (cameraEnabled)
        {
            // setup rotation of sticks
            if (!enableSmoothCam)
            {
                inputXAxis = Input.GetAxis("XBOX_Thumbstick_R_X") + Input.GetAxis("PC_Mouse_X");
                inputYAxis = Input.GetAxis("XBOX_Thumbstick_R_Y") + Input.GetAxis("PC_Mouse_Y");
            }
            cameraX += inputXAxis * inputSensitivity;
            cameraY -= inputYAxis * inputSensitivity;

            cameraY = Mathf.Clamp(cameraY, lowClampAngle, highClampAngle);
        }
        
        if (inputXAxis == 0f && inputYAxis == 0f)
        {
            // Debug.Log("Cam Still");
            if (!playerNotMovingCam)
            {
                // Debug.Log("InitTimer");
                initAdaptiveTimer = Time.time;
                playerNotMovingCam = true;
            }
            if (currentAdaptiveTimer > initAdaptiveTimer + adaptiveDelay && !doAdaptiveCam)
            {
                // Debug.Log("Do Adaptive Cam!");
                doAdaptiveCam = true;
            }
            currentAdaptiveTimer = Time.time;
        }
        else
        {
            // Debug.Log("Moving Cam!");
            doAdaptiveCam = false;
            playerNotMovingCam = false;
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
        transform.position = focusPoint.position + rotation * direction;
        transform.LookAt(focusPoint.position);
    }
    private void FixedUpdate()
    {
        if (enableSmoothCam)
        {
            float initXAxis = Input.GetAxis("XBOX_Thumbstick_R_X") + Input.GetAxis("PC_Mouse_X");
            float initYAxis = Input.GetAxis("XBOX_Thumbstick_R_Y") + Input.GetAxis("PC_Mouse_Y");

            inputXAxis += initXAxis * inputSensitivity * smoothSensitivityMultiplier;
            inputYAxis += initYAxis * inputSensitivity * smoothSensitivityMultiplier;
            
            if (Mathf.Abs(inputXAxis) > smoothCamRound)
            {
                inputXAxis *= smoothCamDecay;
            }
            else
            {
                inputXAxis = 0;
            }

            if (Mathf.Abs(inputYAxis) > smoothCamRound)
            {
                inputYAxis *= smoothCamDecay;
            }
            else
            {
                inputYAxis = 0;
            }
        }
        if (doAdaptiveCam && allowAdaptiveCam)
        {
            AdaptiveCamera();
        }
    }

    // modify camera values based on player's speed
    void SpeedCameraChange()
    {
        // take dot product of player's movement relative to camera direction to apply speed cam only in forward and backwards movement
        float vectorDot = Vector3.Dot(playerObject.GetComponent<PlayerMovement>().GetVelocity().normalized, transform.forward.normalized);
        // modifies camera field of view
        if (modifyFOV)
        {
            GetComponent<Camera>().fieldOfView = defaultFOV + (playerRB.velocity.magnitude * fovSpeedModifier) * vectorDot;
        }
    }
    void AdaptiveCamera()
    {
        float angle = Vector3.Angle(playerObject.GetComponent<PlayerMovement>().GetVelocity().normalized, transform.forward.normalized);
        Vector3 deltaVector = playerObject.GetComponent<PlayerMovement>().GetVelocity().normalized - transform.forward.normalized;
        Vector3 planeVector = Vector3.zero - transform.forward.normalized;
        cameraY -= deltaVector.y * playerObject.GetComponent<PlayerMovement>().GetVelocity().magnitude * adaptiveStrength;
        // Debug.Log(playerObject.GetComponent<PlayerMovement>().GetVelocity().magnitude);
        
    }
    // camera collides with environment using sphere cast
    void CameraCollision()
    {
        RaycastHit hit;
        Vector3 direction = transform.position - playerObject.transform.position;
        
        // check sphere cast
        if (Physics.SphereCast(playerObject.transform.position, sphereCastRadius, direction, out hit, defaultDistance))
        {
            // sphere cast only if the collider is an appropriate tag
            if (hit.collider.tag == "Water" || hit.collider.tag == "Terrain")
            {
                collisionDistance = (hit.point - playerObject.transform.position).magnitude;
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
