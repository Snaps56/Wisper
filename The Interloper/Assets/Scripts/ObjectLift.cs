using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLift : MonoBehaviour {

    public float throwForce = 1000;
    public float pickupRadius = 3.0f;
    public Transform playerCam;
    public static int numPickableOnMap = 100;
    public float maxLiftHeight = 2f;
    public float liftPerSecond = 0.1f;
    public float throwInterval = 0.2f;

    private bool rightMouseDown = false;
    private bool leftMouseDown = false;
    private GameObject[] pickupableObjects = new GameObject[numPickableOnMap];

    private bool isThrowInterval = false;
    private float currentThrowInterval = 0.0f;
    private Vector3 movementVector;
    
    // Use this for initialization
    void Start () {
        pickupableObjects = GameObject.FindGameObjectsWithTag("PickUp");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rightMouseDown = true;
        }
        if (rightMouseDown && Input.GetMouseButtonUp(1))
        {
            rightMouseDown = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftMouseDown = true;
        }
        if (leftMouseDown && Input.GetMouseButtonUp(0))
        {
            leftMouseDown = false;
        }

        int numObjectsWithinRange = 0;
        // check how many pickable objects are nearby
        for (int i = 0; i < pickupableObjects.Length; i++)
        {
            float objectRange = (pickupableObjects[i].transform.position - transform.position).magnitude;
            if (objectRange <= pickupRadius)
            {
                numObjectsWithinRange++;
            }
        }

        // generate an array of pickable objects within range
        GameObject[] objectsWithinRange = new GameObject[numObjectsWithinRange];
        for (int i = 0; i < objectsWithinRange.Length; i++)
        {
            for (int j = 0; j < pickupableObjects.Length; j++)
            {
                float objectRange = (pickupableObjects[j].transform.position - transform.position).magnitude;
                if (objectRange <= pickupRadius)
                {
                    objectsWithinRange[i] = pickupableObjects[j];
                }
            }
        }
        if (rightMouseDown && !isThrowInterval)
        {
            // using normal third person camera
            // Vector3 forceVector = (playerCam.forward.normalized + playerCam.up.normalized) * throwForce + new Vector3(0, 10, 0);

            //using fixed camera
            Vector3 deltaMovementVector = transform.position - movementVector;
            Vector3 forceVector = (deltaMovementVector) * GetComponent<Rigidbody>().velocity.magnitude * throwForce;
            for (int i = 0; i < objectsWithinRange.Length; i++)
            {
                objectsWithinRange[i].GetComponent<Rigidbody>().AddForce(forceVector);
            }
            isThrowInterval = true;
        }
        movementVector = transform.position;
        /*
        if (leftMouseDown)
        {
            for (int i = 0; i < objectsWithinRange.Length; i++)
            {
                objectsWithinRange[i].transform.parent = playerCam;
                objectsWithinRange[i].transform.position += new Vector3(0, maxLiftHeight, 0);
                // objectsWithinRange[i].GetComponent<Rigidbody>().AddForce(Vector3.up * 8);
            }
        }
        else
        {
            transform.parent = null;
        }
        */
        // prevent objects from being immediately picked up after throwing
        if (isThrowInterval)
        {
            currentThrowInterval -= Time.deltaTime;
            if (currentThrowInterval <= 0)
            {
                isThrowInterval = false;
                currentThrowInterval = throwInterval;
            }
        }
    }
}
