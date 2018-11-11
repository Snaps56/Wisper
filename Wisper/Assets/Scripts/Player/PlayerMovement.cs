using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public GameObject mainCamera;
    public float movementSpeed;
    public float sprintMultiplier;
    public float stopMultiplier;
    private Rigidbody rb;
    private bool sprintMod;
    private float finalSpeed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        finalSpeed = movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(mainCamera.transform.right * Input.GetAxis("MovementX"));
        // transform.Translate(mainCamera.transform.forward * Input.GetAxis("MovementY"));

        if (sprintMod == false && (Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint")))
        {
            sprintMod = true;
            finalSpeed = movementSpeed * sprintMultiplier;
        }
        else if (Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint"))
        {
            sprintMod = false;
            finalSpeed = movementSpeed;
        }

        rb.AddForce(mainCamera.transform.right * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        rb.AddForce(mainCamera.transform.forward * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(mainCamera.transform.forward * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(mainCamera.transform.up * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(mainCamera.transform.up * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);

        rb.AddForce(-rb.velocity * stopMultiplier);

    }
}
