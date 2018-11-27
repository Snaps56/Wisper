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
        // If there is a follow target, sets rigid body's velocity to a "following" velocity.
        if (followTarget != null)
        {
            SetFollowTargetVelocity(followTarget);
        }

		movementSpeed = originalMoveSpeed + (1 * orbCountScript.GetOrbCount() * orbMovementIncrease);

        // check if player is pressing the sprint button
        if (sprintMod == false && (Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint")))
        {
            sprintMod = true;
            //finalSpeed = movementSpeed * sprintMultiplier;
        }
        else if (Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint"))
        {
            sprintMod = false;
            //finalSpeed = movementSpeed;
        }

        // if the player is pressing the sprint button, increase the player's movement speed
        if (sprintMod) {
			finalSpeed = movementSpeed * sprintMultiplier;
		} else {
			finalSpeed = movementSpeed;
		}

        // toggle player movement
        if (movementToggledOff == false)
        {
            MovePlayer();
        }
        // debug key for testing toggle movement
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(movementToggledOff);
            ToggleMovement();
        }

        // add an opposing force that will automatically slow down the player and add a "cap" to force applied
        rb.AddForce(-rb.velocity * stopMultiplier);
    }
    // move player, function is called only when movement is toggled
    void MovePlayer()
    {
        // add forces to the player's rigidbody in all 3 movement axis to move the player
        rb.AddForce(mainCamera.transform.right * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        rb.AddForce(mainCamera.transform.forward * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(mainCamera.transform.forward * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(mainCamera.transform.up * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(mainCamera.transform.up * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);

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
        Vector3 toTargetVec = (target.transform.position - transform.position);
        float toTargetMag = toTargetVec.magnitude;
        Vector3 toTargetNorm = toTargetVec.normalized;

        float insideFollowDistance = 0.5f;   // Within this distance, don't apply any following forces so that player isn't forced ontop of npc
        float strongForceTetherDistance = 2f;   // Past this distance, apply stronger forces to move player to npc. Between this and insideFollowDistance, apply small force like standard movement

        Vector3 velocityModifier = new Vector3(1,0,0);   //Vector magnitude 1. If npc has rigid body, make this their velocity. Used to scale "walking" vector when between strongForceTether and insideFollow
        if(target.GetComponent<Rigidbody>() != null)
        {
            velocityModifier = 60 * target.GetComponent<Rigidbody>().velocity;
        }
        
        if(toTargetMag > insideFollowDistance)  // Only applies forces when beyond the insideFollowDistance
        {
            if(toTargetMag <= strongForceTetherDistance)
            {
                rb.AddForce(velocityModifier.magnitude * toTargetNorm); // Add a simple force to keep player moving with target. Applies force with mag based on target velocity in direction of target from player
            }
            else
            {
                rb.AddForce((toTargetMag - strongForceTetherDistance + 1 + velocityModifier.magnitude) * toTargetNorm); // Adds a force that scales linearly with distance from target. At boundary, starts at 1 + velocityModifier's magnitude
            }
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
