using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrow : MonoBehaviour {

    [Header("Game Objects")]
    public Rigidbody playerCharacter;
    public Collider radiusCollider;

    [Header("Throw Mechanics")]
    public float throwForce;
    public float maxObjectThrowSpeed;
    public float liftComboMultiplier;
    public float verticalAimAngle;

    private bool isLiftingObjects = false;
    private bool isThrowingObjects = false;
    private Vector3 movementVector;
    private Vector3 deltaMovementVector;
    private Vector3 forceVector;
    private float currentPlayerVelocity;

    // private float highestVelocity = 0; // used to test max object velocity
    
    // Use this for initialization
    void Start () {
        movementVector = transform.position;
    }

    // Update is called once per frame
    void Update()
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

        currentPlayerVelocity = playerCharacter.velocity.magnitude;
        if (currentPlayerVelocity > 0)
        {
            deltaMovementVector = (transform.position - movementVector).normalized;
        }
        movementVector = transform.position;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp")
        {
            throwObject(other);
        }
    }
    void throwObject(Collider other)
    {
        float objectVelocity = other.GetComponent<Rigidbody>().velocity.magnitude;
        /* used to test max object velocity
        if (objectVelocity > highestVelocity)
        {
            highestVelocity = currentPlayerVelocity;
            Debug.Log(highestVelocity);
        }
        */
        if (isThrowingObjects)
        {
            // use character's direction and speed to determine throw direction and throw strength

            if (objectVelocity < maxObjectThrowSpeed)
            {
                float finalLiftMultiplier = 1;
                if (isLiftingObjects)
                {
                    finalLiftMultiplier = liftComboMultiplier;
                }
                float throwAngle = verticalAimAngle / 90;
                forceVector = (deltaMovementVector + new Vector3(0, throwAngle, 0)) * throwForce * finalLiftMultiplier;
                other.GetComponent<Rigidbody>().AddForce(forceVector);
            }
        }
    }
}
