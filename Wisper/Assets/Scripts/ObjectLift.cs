using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLift : MonoBehaviour {

    [Header("Game Objects")]
    public GameObject character;
    public SphereCollider radiusCollider;
    private Mallory movementScript;

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
        movementScript = character.GetComponent<Mallory>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLiftingObjects = true;
        }
        if (isLiftingObjects && Input.GetMouseButtonUp(1))
        {
            isLiftingObjects = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isThrowingObjects = true;
        }
        if (isThrowingObjects && Input.GetMouseButtonUp(0))
        {
            isThrowingObjects = false;
        }

        if (isLiftingObjects && !isThrowingObjects)
        {
            targetPosition = transform.position + new Vector3(0, liftHeight, 0);
            liftObjects();
        }
        else
        {
            dropObjects();
        }
        currentCharacterVector = movementScript.currentMovementForce;
        currentCharacterVector.y *= 0;
        currentCharacterSpeed = character.GetComponent<Rigidbody>().velocity.magnitude;
    }
    void liftObjects()
    {
        Debug.Log(liftedObjects.Count);
        for (int i = 0; i < liftedObjects.Count; i++)
        {
            float objectDistance = (targetPosition - liftedObjects[i].transform.position).magnitude;
            Vector3 toCenterVector = (targetPosition - liftedObjects[i].transform.position);

            Rigidbody liftedObjectRB = liftedObjects[i].GetComponent<Rigidbody>();
            
            if (liftedObjectRB.velocity.magnitude < liftedObjectMaxSpeed + currentCharacterSpeed)
            {
                liftedObjectRB.AddForce(currentCharacterVector * predictCharacterForceMultiplier);
                liftedObjectRB.AddForce(toCenterVector * liftCenterStrength);
                if (currentCharacterVector.x == 0)
                {
                    Vector3 zeroedX = new Vector3(1, 0, 0);
                    liftedObjectRB.AddForce(liftedObjectRB.velocity.magnitude * zeroedX);
                }
                if (currentCharacterVector.z == 0)
                {
                    Vector3 zeroedZ = new Vector3(0, 0, 1);
                    liftedObjectRB.AddForce(liftedObjectRB.velocity.magnitude * zeroedZ);
                }
            }
            else
            {
                liftedObjectRB.AddForce(-liftedObjectRB.velocity);
            }

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
        if (other.tag == "PickUp")
        {
            if (isLiftingObjects)
            {
                addToLiftedObjects(other);
            }
        }
    }
    void addToLiftedObjects(Collider other)
    {
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
        // liftedObjects.Add(other.transform.gameObject);
    }
}
