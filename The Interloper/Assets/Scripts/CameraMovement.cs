using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    public float turnSpeed = 4.0f;
    public Transform player;
    public float distance = 5.0f;
    public float height = 3.0f;

    private Vector3 offsetX;
    private Vector3 offsetY;

    void Start()
    {

        offsetX = new Vector3(0, height, distance);
        offsetY = new Vector3(0, 0, 0);
    }

    void LateUpdate()
    {
        offsetX = Quaternion.AngleAxis(Input.GetAxis("HorizontalR") * turnSpeed, Vector3.up) * offsetX;
        offsetY = Quaternion.AngleAxis(Input.GetAxis("VerticalR") * turnSpeed, Vector3.right) * offsetY;
        if(offsetY.z > -0.1f)
        {
            offsetY.z = -0.1f;
        }
        transform.position = player.position + offsetY + offsetX;   
        transform.LookAt(player.position);

    }
}