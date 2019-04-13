using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerMovement : MonoBehaviour {

    public GameObject mainCamera;
    public float movementSpeed;
    public float sprintMultiplier;
    public float stopMultiplier;

    public bool vibStop = false;
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
    private bool planarMovementOn = false;

    public ParticleSystem screenParticles;
    public GameObject playerDirection;
    private float insideFollowDistance = 0.5f; // Within this distance, don't apply any following forces so that player isn't forced ontop of npc
    private float strongForceTetherDistance = 2f; // Past this distance, apply stronger forces to move player to npc. Between this and insideFollowDistance, apply small force like standard movement

	private bool isMoving = false;

	public GameObject[] freeModeTrails;

    // Use this for initialization
    void Start()
    {
        // initialize the player's position if they return from the city
        if ((bool)PersistantStateData.persistantStateData.stateConditions["TutorialGateTravelled"])
        {
            transform.position = GameObject.Find("PlayerReturningPosition").transform.position;
        }
        orbCountScript = GetComponentInChildren<OrbCount>();
        rb = GetComponent<Rigidbody>();
        finalSpeed = movementSpeed;
        originalMoveSpeed = movementSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        movementSpeed = originalMoveSpeed + (1 * orbCountScript.GetOrbCount() * orbMovementIncrease);

		//Debug.Log ("Planar Movement: " + planarMovementOn);

        SprintCheck();

        PlanarMovementCheck();
        
        // If there is a follow target, sets rigid body's velocity to a "following" velocity.
        if (followTarget != null)
        {
            SetFollowTargetVelocity(followTarget);
        }

        // Debug key for testing PSD
        if (Input.GetKeyDown(KeyCode.L))
        {
            PersistantStateData.persistantStateData.ToggleDebugMode();
        }
    }

    // Physics-based Update
    void FixedUpdate()
    {
        // toggle player movement, move player using forces if true
        if (!movementToggledOff)
        {
            if (planarMovementOn)
            {
                PlanarMovement();
            }
            else
            {
                MovePlayer();
            }
        }

        // add an opposing force that will automatically slow down the player and add a "cap" to force applied
        rb.AddForce(-rb.velocity * stopMultiplier);
    }
    void PlanarMovementCheck()
    {
        // toggle player movement relative to camera or relative to plane
        if (Input.GetButtonDown("PC_Key_ToggleMode") || Input.GetButtonDown("XBOX_Thumbstick_R_Click"))
        {
            Debug.Log("PlanarMovement: " + planarMovementOn);
            if (planarMovementOn)
            {
                planarMovementOn = false;
				foreach (GameObject trail in freeModeTrails) {
					trail.GetComponent<TrailRenderer> ().emitting = true;
				}
            }
            else
            {
                planarMovementOn = true;
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
                
            }
            else
            {
                sprintMod = false;
                
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
                //GamePad.SetVibration(playerIndex, 0f, 0.5f);
                //vibStop = false;
            }
        }
        else
        {
            finalSpeed = movementSpeed;
			if (screenParticles.isPlaying) {
				screenParticles.Stop ();
                //if (vibStop == false){
                //    GamePad.SetVibration(playerIndex, 0f, 0f);
                //    vibStop = true;
                //}
                
            }
        }

    }

    void PlanarMovement()
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

        Vector3 planarVector = mainCamera.transform.right.normalized * Input.GetAxis("XBOX_Thumbstick_L_X");
        planarVector += mainCamera.transform.right.normalized * Input.GetAxis("PC_Axis_MovementX");

        Vector3 forwardVector = mainCamera.transform.forward.normalized;
        forwardVector.y = 0;
        forwardVector = forwardVector.normalized;

        planarVector += forwardVector.normalized * Input.GetAxis("XBOX_Thumbstick_L_Y");
        planarVector += forwardVector.normalized * Input.GetAxis("PC_Axis_MovementZ");

        planarVector += Vector3.up.normalized * Input.GetAxis("XBOX_Axis_MovementY");
        planarVector += Vector3.up.normalized * Input.GetAxis("PC_Axis_MovementY");

        planarVector = planarVector.normalized * finalSpeed;

		if (planarVector.magnitude > 0) {
			Quaternion playerRotation = Quaternion.LookRotation (planarVector, Vector3.up);
			playerDirection.transform.rotation = playerRotation;
		}

        rb.AddForce(planarVector);
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
        if (GetVelocity().magnitude > .1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
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
                rb.AddForce((Mathf.Pow((toTargetMag - strongForceTetherDistance),2) + 2 + velocityModifier.magnitude) * toTargetNorm); // Adds a force that scales linearly with distance from target. At boundary, starts at 1 + velocityModifier's magnitude
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
        Debug.Log("Player movement script toggled movement");
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
        //Debug.Log("Player movement script enabled movement");
        movementToggledOff = false;
    }
    public void DisableMovement()
    {
        //Debug.Log("Player movement script disabled movement");
        movementToggledOff = true;
    }
    // Return whether if player is able to move
    public bool GetToggleMovement()
    {
        return movementToggledOff;
    }

}
