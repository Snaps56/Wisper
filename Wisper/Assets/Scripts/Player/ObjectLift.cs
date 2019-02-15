using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ObjectLift : MonoBehaviour {

    [Header("Game Objects")]
    public GameObject character;
    public SphereCollider radiusCollider;
    private OrbCount orbcountScript;
	private float playerOrbCount;
	// Particles that play when lift button is pressed
	public ParticleSystem liftParticles;

    [Header("Lift Mechanics")]
    public float liftHeight;
    public float liftCenterStrength;
    public float liftForceSlowDown;
    public float maxHoldRadiusMultiplier;

    private bool isLiftingObjects = false;

    private bool isThrowingObjects;
    private Vector3 targetPosition;
    private Vector3 currentCharacterVector;
    private float currentCharacterSpeed;
	// Base lift strength
	private float originalLiftCenterStrength;

    //Vibrate Settings
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    List<GameObject> liftedObjects = new List<GameObject>();

    // Use this for initialization
    void Start () {
        orbcountScript = character.GetComponentInChildren<OrbCount>();
		playerOrbCount = orbcountScript.GetOrbCount ();
		// Initialize original lift strength
		originalLiftCenterStrength = liftCenterStrength;
    }

    // Update is called once per frame
    void Update()
    {
        // check if the player is currently pressing the lift button
        if (Input.GetButton("PC_Mouse_Click_R") || Input.GetAxis("XBOX_Trigger_L") > 0)
        {
            isLiftingObjects = true;
          //  GamePad.SetVibration(playerIndex, 0f, state.Triggers.Left);
        }
        if (isLiftingObjects && !Input.GetButton("PC_Mouse_Click_R") && Input.GetAxis("XBOX_Trigger_L") <= 0)
        {
            isLiftingObjects = false;
          //  GamePad.SetVibration(playerIndex, 0f, 0f);
            //if (GetComponent<PlayerMovement>().GetIsSprinting() == true)
            //{
            //    GamePad.SetVibration(playerIndex, 0f, 0.5f);
            //    Debug.Log("IsSprinting = true");
            //}
            //else if (GetComponent<PlayerMovement>().GetIsSprinting() == false)
            //{
            //    GamePad.SetVibration(playerIndex, 0f, 0f);
            //    Debug.Log("IsSprinting = false");
            //}
        }
        isThrowingObjects = character.GetComponentInChildren<ObjectThrow>().GetIsThrowingObjects();

        // Modify lift strength based on orb count
        playerOrbCount = orbcountScript.GetOrbCount();
        liftCenterStrength = originalLiftCenterStrength + (2 * playerOrbCount);

        // obtain character movement data to help track movement for lifted objects
        // currentCharacterVector = orbcountScript.currentMovementForce;
        currentCharacterVector.y *= 0;
        currentCharacterSpeed = character.GetComponent<Rigidbody>().velocity.magnitude;

    //    // Find a PlayerIndex, for a single player game
    //    // Will find the first controller that is connected and use it
    //    if (!playerIndexSet || !prevState.IsConnected)
    //    {
    //        for (int i = 0; i < 4; ++i)
    //        {
    //            PlayerIndex testPlayerIndex = (PlayerIndex)i;
    //            GamePadState testState = GamePad.GetState(testPlayerIndex);
    //            if (testState.IsConnected)
    //            {
    //                //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
    //                playerIndex = testPlayerIndex;
    //                playerIndexSet = true;
    //            }
    //        }
    //    }

    //    prevState = state;
    //    state = GamePad.GetState(playerIndex);

    }
    void FixedUpdate ()
    {
        // lift objects when not throwing
		// Play particles when player is holding the lift button
        if (isLiftingObjects && !isThrowingObjects)
        {
            targetPosition = transform.position + new Vector3(0, liftHeight, 0);
            liftObjects();
			if (!liftParticles.isPlaying) {
				liftParticles.Play ();
			}
        }
        else
        {
            dropObjects();
			if (liftParticles.isPlaying) {
				liftParticles.Stop ();
			}
        }
    }

    // returns bool that checks if the player is currently trying to lift objects
    public bool GetIsLiftingObjects()
    {
        return isLiftingObjects;
    }

    // lift a nearby pickable object when the player presses the lift button
    void liftObjects()
    {
        // use a for loop for every nearby object detected
        for (int i = 0; i < liftedObjects.Count; i++)
        {
            // calculate the force needed to lift an object toward the player
            float objectDistance = (targetPosition - liftedObjects[i].transform.position).magnitude;
            Vector3 toCenterVector = (targetPosition - liftedObjects[i].transform.position);

            Rigidbody liftedObjectRB = liftedObjects[i].GetComponent<Rigidbody>();
            
            // add force to object only if its not moving too fast
            liftedObjectRB.AddForce(toCenterVector * liftCenterStrength); // orbit to center force
            liftedObjectRB.AddForce(-liftedObjectRB.velocity * liftForceSlowDown); // orbit to center force
            liftedObjectRB.AddTorque(new Vector3(1, 0, 0));
            liftedObjectRB.AddTorque(-liftedObjectRB.angularVelocity);

            // remove lifted object if it strays to far from the player
            if (objectDistance > radiusCollider.radius * maxHoldRadiusMultiplier)
            {
                liftedObjects.Remove(liftedObjects[i]);
            }

        }
    }

    // drop a nearby pickable object when the player releases the lift button
    void dropObjects()
    {
        liftedObjects.Clear();
        liftedObjects.TrimExcess();
    }

    // detect if any pickable objects are nearby
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp")
        {
            // if the player is using the lift ability, add the object to an array of lifted objects
            if (isLiftingObjects)
            {
                addToLiftedObjects(other);
            }
        }
    }

    public float GetLiftForce()
    {
        return liftCenterStrength;
    }

    // add an object to the current array of lifted objects
    void addToLiftedObjects(Collider other)
    {
        if (liftedObjects.Count < 1)
        {
            liftedObjects.Add(other.gameObject);
        }
        else
        {
            // double check if the new lifted object is not the same as another object already in the array
            for (int i = 0; i < liftedObjects.Count; i++)
            {
                if (liftedObjects.IndexOf(other.gameObject) == -1)
                {
                    liftedObjects.Add(other.gameObject);
                }
            }
        }
    }
}
