using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour {

    public GameObject mainCamera;
    public float movementSpeed;
    public float sprintMultiplier;
    public float stopMultiplier;

    public bool VibStop = false;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private Rigidbody rb;
    private bool sprintMod;
    private float finalSpeed;
    public float flightSpeedMultiplier;

    private OrbCount orbCountScript;
    public float orbMovementIncrease;

    private float originalMoveSpeed;
    private float targetFollowDistance = 5;
    private GameObject followTarget;

    private bool movementToggledOff = false;
    private bool planalMovementOn = true;

    public ParticleSystem screenParticles;
    public GameObject playerDirection;
    private float insideFollowDistance = 0.5f; // Within this distance, don't apply any following forces so that player isn't forced ontop of npc
    private float strongForceTetherDistance = 2f; // Past this distance, apply stronger forces to move player to npc. Between this and insideFollowDistance, apply small force like standard movement

	private bool isMoving = false;

	public GameObject[] freeModeTrails;

    // Use this for initialization
    void Start()
    {
        orbCountScript = GetComponentInChildren<OrbCount>();
        rb = GetComponent<Rigidbody>();
        finalSpeed = movementSpeed;
        originalMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        movementSpeed = originalMoveSpeed + (1 * orbCountScript.GetOrbCount() * orbMovementIncrease);

		//Debug.Log ("Planar Movement: " + planalMovementOn);

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
        if (Input.GetButtonDown("PC_Key_Control") || Input.GetButtonDown("XBOX_Thumbstick_R_Click"))
        {
            Debug.Log("PlanalMovement: " + planalMovementOn);
            if (planalMovementOn)
            {
                planalMovementOn = false;
				foreach (GameObject trail in freeModeTrails) {
					trail.GetComponent<TrailRenderer> ().emitting = true;
				}
            }
            else
            {
                planalMovementOn = true;
				foreach (GameObject trail in freeModeTrails) {
					trail.GetComponent<TrailRenderer> ().emitting = false;
				}
            }
        }
    }
    void SprintCheck()
    {
		//Debug.Log ("Accel: " + GetIsAccelerating ());
		//Debug.Log ("Stationary: " + GetIsStationary ());
        // check if player is pressing the sprint button
		if ((Input.GetButtonDown("XBOX_Thumbstick_L_Click") || Input.GetButtonDown("PC_Key_Sprint")) && GetIsAccelerating())
        {
            //Debug.Log("Sprinting: " + sprintMod);
            if (!sprintMod)
            {
                sprintMod = true;
                GamePad.SetVibration(playerIndex, 0f, 1f);
            }
            else
            {
                sprintMod = false;
                GamePad.SetVibration(playerIndex, 0f, 0f);
            }
        }

		if (!GetIsAccelerating()) {
			sprintMod = false;
		}

        // if the player is pressing the sprint button, increase the player's movement speed
        if (sprintMod)
        {
            finalSpeed = movementSpeed * sprintMultiplier;
			if (!screenParticles.isPlaying) {
				screenParticles.Play ();
			}
        }
        else
        {
            finalSpeed = movementSpeed;
			if (screenParticles.isPlaying) {
				screenParticles.Stop ();
			}
        }

    }

    void PlanalMovement()
    {
        /*
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        Vector3 forwardVector = mainCamera.transform.forward.normalized;
        forwardVector.y = 0;
        forwardVector = forwardVector.normalized;

        rb.AddForce(forwardVector.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(forwardVector.normalized * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(Vector3.up.normalized * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(Vector3.up.normalized * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);
        */

        Vector3 planalVector = mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X");
        planalVector += mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX");

        Vector3 forwardVector = mainCamera.transform.forward.normalized;
        forwardVector.y = 0;
        forwardVector = forwardVector.normalized;

        planalVector += forwardVector.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y");
        planalVector += forwardVector.normalized * Input.GetAxis("PC_Axis_MovementZ");

        planalVector += Vector3.up.normalized * Input.GetAxis("XBOX_Axis_MovementY");
        planalVector += Vector3.up.normalized * Input.GetAxis("PC_Axis_MovementY");

        planalVector = planalVector.normalized * finalSpeed;

		if (planalVector.magnitude > 0) {
			Quaternion playerRotation = Quaternion.LookRotation (planalVector, Vector3.up);
			playerDirection.transform.rotation = playerRotation;
		}

        rb.AddForce(planalVector);
    }

    // move player, function is called only when movement is toggled
    void MovePlayer()
    {
        // this commented block has diagonal movement moving faster than normal directions
        /*
        // add forces to the player's rigidbody in all 3 movement axis to move the player
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X") * finalSpeed);
        rb.AddForce(mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX") * finalSpeed);

        rb.AddForce(mainCamera.transform.forward.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y") * finalSpeed);
        rb.AddForce(mainCamera.transform.forward.normalized * Input.GetAxis("PC_Axis_MovementZ") * finalSpeed);

        rb.AddForce(mainCamera.transform.up.normalized * Input.GetAxis("XBOX_Axis_MovementY") * finalSpeed);
        rb.AddForce(mainCamera.transform.up.normalized * Input.GetAxis("PC_Axis_MovementY") * finalSpeed);
        */


        // diagonal movement is same speed as normal direction movement

        Vector3 movementVector = mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X");
        movementVector += mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX");

        movementVector += mainCamera.transform.forward.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y");
        movementVector += mainCamera.transform.forward.normalized * Input.GetAxis("PC_Axis_MovementZ");

        movementVector += mainCamera.transform.up.normalized * Input.GetAxis("XBOX_Axis_MovementY");
        movementVector += mainCamera.transform.up.normalized * Input.GetAxis("PC_Axis_MovementY");

        movementVector = movementVector.normalized * finalSpeed * flightSpeedMultiplier;

		if (movementVector.magnitude > 0) {
			Quaternion playerRotation = Quaternion.LookRotation (movementVector, Vector3.up);
			playerDirection.transform.rotation = playerRotation;
		}

        rb.AddForce(movementVector);
    }

    // returns the player's current velocity
    public Vector3 GetVelocity()
    {
        return rb.velocity;
    }
    public float GetSidewaysAxis()
    {
        float sidewaysAxis = Input.GetAxis("XBOX_Thumbstick_L_X");
        sidewaysAxis += Input.GetAxis("PC_Axis_MovementX");

        return sidewaysAxis;
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

	public bool GetIsAccelerating() {
		if (GetForce ().magnitude != 0) {
			return true;
		} else {
			return false;
		}
	}

	public bool GetIsStationary() {
		if (GetVelocity ().magnitude < .1) {
			return true;
		} else {
			return false;
		}
	}

    // returns a boolean that checks whether if the player is sprinting or not
    public bool GetIsSprinting()
    {
        return sprintMod;
    }

	public bool GetIsMoving() {
		return isMoving;
	}

    // Sets the velocity so that player follows a target. Use in the update block for proper functionality.
    public void SetFollowTargetVelocity(GameObject target)
    {
        Vector3 toTargetVec = (target.transform.position - transform.position);
        float toTargetMag = toTargetVec.magnitude;
        Vector3 toTargetNorm = toTargetVec.normalized;

        // Within this distance, don't apply any following forces so that player isn't forced ontop of npc
        // Past this distance, apply stronger forces to move player to npc. Between this and insideFollowDistance, apply small force like standard movement

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
                rb.AddForce((Mathf.Pow((toTargetMag - strongForceTetherDistance),3) + 2 + velocityModifier.magnitude) * toTargetNorm); // Adds a force that scales linearly with distance from target. At boundary, starts at 1 + velocityModifier's magnitude
            }
        }
    }

    public void SetFollowTarget(GameObject target, float tetherMin = 0.5f, float tetherStrong = 2f)
    {
        followTarget = target;
        insideFollowDistance = tetherMin;
        strongForceTetherDistance = tetherStrong;
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
    public void EnableMovement()
    {
        movementToggledOff = false;
    }
    public void DisableMovement()
    {
        movementToggledOff = true;
    }
    // Return whether if player is able to move
    public bool GetToggleMovement()
    {
        return movementToggledOff;
    }

}
