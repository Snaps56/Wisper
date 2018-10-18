using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLift : MonoBehaviour {

    [Header("Game Objects")]
    public GameObject character;
    public SphereCollider radiusCollider;
    private Player movementScript;

    [Header("Lift Mechanics")]
    public float liftHeight;
    public float liftCenterStrength;
    public float liftedObjectMaxSpeed;
    public float predictCharacterForceMultiplier;
    public float maxHoldRadiusMultiplier;

    private bool isLiftingObjects = false;
    private bool isThrowingObjects = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 currentCharacterVector;
    private float currentCharacterSpeed;
    List<GameObject> liftedObjects = new List<GameObject>();

    // Use this for initialization
    void Start () {
        movementScript = character.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // check right mouse click
        if (Input.GetMouseButtonDown(1))
        {
            isLiftingObjects = true;
        }
        if (isLiftingObjects && Input.GetMouseButtonUp(1))
        {
            isLiftingObjects = false;
        }

        // check left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            isThrowingObjects = true;
        }
        if (isThrowingObjects && Input.GetMouseButtonUp(0))
        {
            isThrowingObjects = false;
        }

        // lift objects when not throwing
        if (isLiftingObjects && !isThrowingObjects)
        {
            targetPosition = transform.position + new Vector3(0, liftHeight, 0);
            liftObjects();
        }
        else
        {
            dropObjects();
        }
        // obtain character movement data to help track movement for lifted objects
        currentCharacterVector = movementScript.currentMovementForce;
        currentCharacterVector.y *= 0;
        currentCharacterSpeed = character.GetComponent<Rigidbody>().velocity.magnitude;
    }
    void liftObjects()
    {
        for (int i = 0; i < liftedObjects.Count; i++)
        {
            // apply force to all objects that are lifted
            float objectDistance = (targetPosition - liftedObjects[i].transform.position).magnitude;
            Vector3 toCenterVector = (targetPosition - liftedObjects[i].transform.position).normalized;

            Rigidbody liftedObjectRB = liftedObjects[i].GetComponent<Rigidbody>();
            
            // add force to object only if its not moving too fast
            if (liftedObjectRB.velocity.magnitude < liftedObjectMaxSpeed + currentCharacterSpeed)
            {
                liftedObjectRB.AddForce(currentCharacterVector * predictCharacterForceMultiplier); // movement prediction force
                liftedObjectRB.AddForce(toCenterVector * liftCenterStrength); // orbit to center force
            }
            else
            {
                // if object is moving to fast, add a cap to its speed
                liftedObjectRB.AddForce(-liftedObjectRB.velocity);
            }

            // remove lifted object if they stray to far from the player
            if (objectDistance > radiusCollider.radius * maxHoldRadiusMultiplier)
            {
                liftedObjects.Remove(liftedObjects[i]);
            }
        }
    }
    void dropObjects()
    {
        liftedObjects.Clear();
        liftedObjects.TrimExcess();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp" || other.tag == "Orb")
        {
            if (isLiftingObjects)
            {
                addToLiftedObjects(other);
            }
        }
    }
    void addToLiftedObjects(Collider other)
    {
        // add object to a list of all lifted objects
        if (liftedObjects.Count < 1)
        {
            liftedObjects.Add(other.gameObject);
        }
        else
        {
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
