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
    public float orbMovementIncrease;

    private float originalMoveSpeed;
    private float targetFollowDistance = 5;
    private GameObject followTarget;

    private bool movementToggledOff = false;
    private bool planalMovementOn = false;

    // Use this for initialization
    void Start()
    {
        orbCountScript = GetComponent<OrbCount>();
        rb = GetComponent<Rigidbody>();
        finalSpeed = movementSpeed;
        originalMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        movementSpeed = originalMoveSpeed + (1 * orbCountScript.GetOrbCount() * orbMovementIncrease);

        SprintCheck();

        PlanalMovementCheck();

        // debug key for testing toggle movement
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(movementToggledOff);
            ToggleMovement();
        }

        // If there is a follow target, sets rigid body's velocity to a "following" velocity.
        if (followTarget != null)
        {
            SetFollowTargetVelocity(followTarget);
        }

    }

    // Physics-based Update
    void FixedUpdate()
    {
        // toggle player movement, move player using forces if true
        if (!movementToggledOff)
        {
            if (planalMovementOn)
            {
                PlanalMovement();
            }
            else
            {
                MovePlayer();
            }
        }
        // add an opposing force that will automatically slow down the player and add a "cap" to force applied
        rb.AddForce(-rb.velocity * stopMultiplier);
    }
    void PlanalMovementCheck()
    {
        // toggle player movement relative to camera or relative to plane
        if (Input.GetButtonDown("PC_Mouse_Click_Mid") || Input.GetButtonDown("XBOX_Thumbstick_R_Click"))
        {
            Debug.Log("PlanalMovement: " + planalMovementOn);
            if (planalMovementOn)
            {
                planalMovementOn = false;
            }
            else
            {
                planalMovementOn = true;
            }
        }
    }
    void SprintCheck()
    {
        // check if player is pressing the sprint button
        if (Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint"))
        {
            Debug.Log("Sprinting: " + sprintMod);
            if (!sprintMod)
            {
                sprintMod = true;
            }
            else
            {
                sprintMod = false;
            }
        }

        // if the player is pressing the sprint button, increase the player's movement speed
        if (sprintMod)
        {
            finalSpeed = movementSpeed * sprintMultiplier;
        }
        else
        {
            finalSpeed = movementSpeed;
        }

    }

    void PlanalMovement()
    {
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        Vector3 forwardVector = mainCamera.transform.forward.normalized;
        forwardVector.y = 0;
        forwardVector = forwardVector.normalized;
        
        rb.AddForce(forwardVector.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(forwardVector.normalized * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(Vector3.up.normalized * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(Vector3.up.normalized * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);
    }

    // move player, function is called only when movement is toggled
    void MovePlayer()
    {

        // add forces to the player's rigidbody in all 3 movement axis to move the player
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        rb.AddForce(mainCamera.transform.forward.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(mainCamera.transform.forward.normalized * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(mainCamera.transform.up.normalized * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(mainCamera.transform.up.normalized * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);

    }

    // returns the player's current velocity
    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }

    // returns the force that the player is applying to the character to move around
    public Vector3 GetForce()
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

    // returns a boolean that checks whether if the player is sprinting or not
    public bool GetIsSprinting()
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

    // Toggle whether if the player can move
    public void ToggleMovement()
    {
        if (movementToggledOff == true)
        {
            movementToggledOff = false;
        }
        else
        {
            movementToggledOff = true;
        }
    }

    // Return whether if player is able to move
    public bool GetToggleMovement()
    {
        return movementToggledOff;
    }

}
