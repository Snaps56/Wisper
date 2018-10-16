using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private const float Y_ANGLE_MIN = -40.0f;
    private const float Y_ANGLE_MAX = 40.0f;

    [Header("Game Objects")]
    public Transform lookAt;
    public Transform camTransform;

    [Header("Camera Mechanics")]
    public float maxDistance = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 22.0f;
    public float sensitivityX = 4.0f;
    public float sensitivityY = 1.0f;

    [Header("Camera Collision")]
    public float minDistance = 1.0f;
    public float collisionSmooth = 10.0f;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float zoomRatio = 1.0f;
    float distance;

    private void Awake()
    {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }
    private void Start()
    {
        camTransform = transform;
    }

    private void Update()
    {
        // controller look
        currentX += Input.GetAxis("HorizontalR");
        currentY += Input.GetAxis("VerticalR");

        // mouse look
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

        CameraCollision();
    }
    void CameraCollision()
    {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * distance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit))
        {
            distance = Mathf.Clamp(hit.distance * zoomRatio, minDistance, distance);
        }
        else
        {
            distance = maxDistance;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir* distance, Time.deltaTime* collisionSmooth);
	}
    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY*sensitivityY, currentX*sensitivityX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
