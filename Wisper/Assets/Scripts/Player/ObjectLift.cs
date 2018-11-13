using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLift : MonoBehaviour {

    [Header("Game Objects")]
    public GameObject character;
    public SphereCollider radiusCollider;
    private Player movementScript;
	private float playerOrbCount;
	public ParticleSystem liftParticles;

    [Header("Lift Mechanics")]
    public float liftHeight;
    public float liftCenterStrength;
    public float liftedObjectMaxSpeed;
    public float predictCharacterForceMultiplier;
    public float maxHoldRadiusMultiplier;

    private bool isLiftingObjects = false;
    private bool isHoldingButton = false;
    private bool isThrowingObjects;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 currentCharacterVector;
    private float currentCharacterSpeed;
	private float originalLiftCenterStrength;
    List<GameObject> liftedObjects = new List<GameObject>();

    // Use this for initialization
    void Start () {
        movementScript = character.GetComponent<Player>();
		playerOrbCount = movementScript.GetOrbCount ();
		originalLiftCenterStrength = liftCenterStrength;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetAxis("TriggerL") > 0)
        {
            isLiftingObjects = true;
        }
        if (isLiftingObjects && !Input.GetMouseButton(1) && Input.GetAxis("TriggerL") <= 0)
        {
            isLiftingObjects = false;
        }

        isThrowingObjects = character.GetComponentInChildren<ObjectThrow>().GetIsThrowingObjects();

        // lift objects when not throwing
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

		playerOrbCount = movementScript.GetOrbCount ();
		liftCenterStrength = originalLiftCenterStrength + (2 * playerOrbCount);

        // obtain character movement data to help track movement for lifted objects
        currentCharacterVector = movementScript.currentMovementForce;
        currentCharacterVector.y *= 0;
        currentCharacterSpeed = character.GetComponent<Rigidbody>().velocity.magnitude;
    }
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
            Vector3 toCenterVector = (targetPosition - liftedObjects[i].transform.position).normalized;

            Rigidbody liftedObjectRB = liftedObjects[i].GetComponent<Rigidbody>();
            
            // add force to object only if its not moving too fast
            if (liftedObjectRB.velocity.magnitude < liftedObjectMaxSpeed + currentCharacterSpeed)
            {
                liftedObjectRB.AddForce(currentCharacterVector * predictCharacterForceMultiplier); // movement prediction force
                liftedObjectRB.AddForce(toCenterVector * liftCenterStrength); // orbit to center force
            }
            // if object is moving to fast, add a cap to its speed that it uses to follow the player
            else
            {
                liftedObjectRB.AddForce(-liftedObjectRB.velocity);
            }

            // remove lifted object that strays too far from the player
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
