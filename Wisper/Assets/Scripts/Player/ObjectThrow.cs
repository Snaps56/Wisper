using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrow : MonoBehaviour {

    [Header("Game Objects")]
    public GameObject character;
    public Collider radiusCollider;
    private OrbCount orbcountScript;
	private float playerOrbCount;
	public ParticleSystem throwParticles;

    [Header("Throw Mechanics")]
    public float throwForce;
    public float maxObjectThrowSpeed;
    public float liftComboMultiplier;
    public float verticalAimAngle;

    private bool isLiftingObjects;
    private bool isThrowingObjects = false;
    private Vector3 movementVector;
    private Vector3 deltaMovementVector;
    private Vector3 forceVector;
    private float currentPlayerVelocity;
	private float originalThrowForce;

    // Use this for initialization
    void Start () {
        playerOrbCount = character.GetComponent<OrbCount> ().GetOrbCount ();
		originalThrowForce = throwForce;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isLiftingObjects = character.GetComponentInChildren<ObjectLift>().GetIsLiftingObjects();

        // check if player is pressing the throw button
        if (Input.GetButton("PC_Mouse_Click_L") || Input.GetAxis("XBOX_Trigger_R") > 0)
        {
            isThrowingObjects = true;
        }
        if (isThrowingObjects && !Input.GetButton("PC_Mouse_Click_L") && Input.GetAxis("XBOX_Trigger_R") <= 0)
        {
            isThrowingObjects = false;
        }
    
        character.GetComponent<Rigidbody>();

		if (isThrowingObjects) {
			if (!throwParticles.isPlaying) {
				throwParticles.Play ();
			}
		} else {
			if (throwParticles.isPlaying) {
				throwParticles.Stop ();
			}
		}

		playerOrbCount = character.GetComponent<OrbCount> ().GetOrbCount ();
		throwForce = originalThrowForce + playerOrbCount;

        // obtain player movement vector to determine throw direction
        currentPlayerVelocity = character.GetComponent<Rigidbody>().velocity.magnitude;
        if (currentPlayerVelocity > 0)
        {
            movementVector = character.GetComponent<PlayerMovement>().GetVelocity();
            deltaMovementVector = (movementVector.normalized - transform.position.normalized).normalized;
            deltaMovementVector.y *= 0;
        }
        // Debug.Log(deltaMovementVector);
    }

	public bool GetIsThrowingObjects() {
		return isThrowingObjects;
    }

    // detect if any pickable objects are within range
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp")
        {
            throwObject(other);
        }
    }

    // throw objects that are within range
    // parameter passed from OnTriggerStay method
    void throwObject(Collider other)
    {
        float objectVelocity = other.GetComponent<Rigidbody>().velocity.magnitude;

        // check if the player is pressing the throw button
        if (isThrowingObjects)
        {
            // apply throw force, however add a speed limit to thrown objects
            if (objectVelocity < maxObjectThrowSpeed)
            {
                // throwing an object while lifting them will grant a throw power bonus
                float finalLiftMultiplier = 1;
                if (isLiftingObjects)
                {
                    finalLiftMultiplier = liftComboMultiplier;
                }

                // throw force determined by last movement input, and thrown at an angle upwards
                float throwAngle = verticalAimAngle / 90;
                forceVector = (deltaMovementVector + new Vector3(0, throwAngle, 0)) * throwForce * finalLiftMultiplier;
                other.GetComponent<Rigidbody>().AddForce(forceVector);
            }
        }
    }
}
