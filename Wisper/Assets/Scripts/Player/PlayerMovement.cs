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
    private OrbCount orbCountScript;
    private float originalMoveSpeed;
    private float targetFollowDistance = 5;
    private GameObject followTarget;


    // Use this for initialization
    void Start()
    {
        orbCountScript = GetComponent<OrbCount>();
        rb = GetComponent<Rigidbody>();
        finalSpeed = movementSpeed;
        originalMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // transform.Translate(mainCamera.transform.right * Input.GetAxis("MovementX"));
        // transform.Translate(mainCamera.transform.forward * Input.GetAxis("MovementY"));

        // If there is a follow target, sets rigid body's velocity to a "following" velocity.
        if (followTarget != null)
        {
            SetFollowTargetVelocity(followTarget);
        }

        movementSpeed = originalMoveSpeed + orbCountScript.GetOrbCount();
        
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
        currentForceVector();
    }

    public Vector3 currentVelocityVector()
    {
        return rb.velocity;
    }

    public float currentVelocityMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public Vector3 currentForceVector()
    {
        Vector3 forceVector = Vector3.zero;
        forceVector += mainCamera.transform.forward * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed;
        forceVector += mainCamera.transform.forward * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed;
        forceVector += mainCamera.transform.right * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed;
        forceVector += mainCamera.transform.right * Input.GetAxis("PC_Axis_MovementX") * finalSpeed;
        forceVector += mainCamera.transform.up * Input.GetAxis("PC_Axis_MovementY") * finalSpeed;
        forceVector += mainCamera.transform.up * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed;
        // Debug.Log(forceVector);
        return forceVector;
    }

    public bool isSprinting()
    {
        return sprintMod;
    }



    // Sets the velocity so that player follows a target. Use in the update block for proper functionality.
    public void SetFollowTargetVelocity(GameObject target)
    {
        Vector3 followVel = new Vector3();
        float verticalError = 2;
        float catchUpMag = 0.05f;
        float yAdjustMag = 0.1f;

        if(target.GetComponent<Rigidbody>() != null)
        {
            /*
            followVel = target.GetComponent<Rigidbody>().velocity;

            //Increases velocity proportial to distance to target if outside the follow distance. A "catch up" feature.
            if (Vector3.Magnitude(transform.position - target.transform.position) > targetFollowDistance)
            {
                followVel += (target.transform.position - transform.position) * catchUpMag; // Adds a percentage of difference between position to velocity vector
            }

            //Alters velocity to move player within a range of vertical distance to the target.
            if (transform.position.y > target.transform.position.y + verticalError || transform.position.y < target.transform.position.y - verticalError)
            {
                float yVal = (target.transform.position.y - transform.position.y) * yAdjustMag; // Adds a percentage of difference in y val to velocity vector
                followVel += new Vector3(0, yVal, 0);
            }
            //Debug.Log("setting follow velocity on rb to x: " + followVel.x  + " y: " + followVel.y + " z: " + followVel.z);
            //rb.velocity.Set(followVel.x, followVel.y, followVel.z);   // Sets the rb velocity to the follow velocity.
            //Debug.Log("rb vel is  " + rb.velocity.x + " y: " + rb.velocity.y + " z: " + rb.velocity.z);
            rb.AddForce(followVel);
            */
            transform.position = target.transform.position - 2 * target.transform.forward + new Vector3(0,1,0);
        } 
    }

    public void SetFollowTarget(GameObject target)
    {
        followTarget = target;
    }

    public void RemoveFollowTarget()
    {
        followTarget = null;
    }
}
