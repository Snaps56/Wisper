using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptimized : MonoBehaviour
{

    [Header("Game Objects")]
    public GameObject cameraObj;
    public GameObject playerObj;

    [Header("Camera Mechanics")]
    public float cameraMoveSpeed;
    public float highClampAngle;
    public float lowClampAngle;
    public float inputSensitivity;

    Vector3 followerPos;
    private float mouseX;
    private float mouseY;
    private float finalInputX;
    private float finalInputY;
    private float rotationY;
    private float rotationX;

    // Use this for initialization
    void Start()
    {
        Vector3 rotation = transform.localRotation.eulerAngles;
        rotationY = rotation.y;
        rotationX = rotation.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // setup rotation of sticks
        float inputX = Input.GetAxis("HorizontalR");
        float inputY = Input.GetAxis("VerticalR");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputY = inputY + mouseY;

        rotationY += finalInputX * inputSensitivity * Time.deltaTime;
        rotationX += finalInputY * inputSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, lowClampAngle, highClampAngle);

        Quaternion localRotation = Quaternion.Euler(rotationX, rotationY, 0.0f);
        transform.rotation = localRotation;
    }
    void LateUpdate()
    {
        CameraUpdater();
    }
    void CameraUpdater()
    {
        // set the target object to follow
        Transform target = playerObj.transform;
        //move toward the game object that is the target
        float step = cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
