using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {

    public GameObject abilitiesCollider;
    public float torqueMultiplier = 0.05f;
    public float baseSpeed = 10f;
    public float dangerSpeed = 6f;
    public float correctSpeed = 2f;
    public GameObject[] windmillParts;

    public int attachCount = 0;

    private Rigidbody rb;

    private bool reachedDangerSpeed = false;
    private bool isThrowing;
    private bool isLifting;

    private float torque;
    private bool isWithinRange = false;
    private float currentVelocity;
    private PersistantStateData persistantStateData;

    private bool hasSpawnedOrbs = false;

    // Use this for initialization
    void Start () {
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        isLifting = abilitiesCollider.GetComponent<ObjectLift>().GetIsLiftingObjects();
        isThrowing = abilitiesCollider.GetComponent<ObjectThrow>().GetIsThrowingObjects();

        //If the windmill is fixed
        if (attachCount == 4)
        {
            if ((isLifting || isThrowing) && isWithinRange)
            {
                torque = abilitiesCollider.GetComponent<ObjectThrow>().GetThrowForce() * 5;
            }
            else if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false)
            {
                torque = 0;
            }
            currentVelocity = rb.angularVelocity.y;
            //Debug.Log("current velocity: " + currentVelocity + ", danger speed: " + dangerSpeed);

            if (currentVelocity >= dangerSpeed && !reachedDangerSpeed)
            {
                Debug.Log("Broken");
                reachedDangerSpeed = true;
                for (int i = 0; i < windmillParts.Length; i++)
                {
                    Vector3 currentShellsterVelocity = windmillParts[i].GetComponent<Rigidbody>().velocity;
                    windmillParts[i].GetComponent<Rigidbody>().isKinematic = false;
                    windmillParts[i].transform.parent = null;
                    windmillParts[i].GetComponent<Rigidbody>().AddExplosionForce(150, transform.position, 5f);
                    Debug.Log("detached " + windmillParts[i]);
                }
            }
            else if (currentVelocity >= correctSpeed && !reachedDangerSpeed && !hasSpawnedOrbs)
            {
                Debug.Log("Complete");
                hasSpawnedOrbs = true;
                GetComponent<SpawnOrbs>().DropOrbs();
                persistantStateData.stateConditions["WindmillTaskDone"] = true;
                Debug.Log("WindmillTaskDone: " + (bool)persistantStateData.stateConditions["WindmillTaskDone"]);
            }
        }




    }
    private void FixedUpdate()
    {
        if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false)
        {
            rb.AddTorque(0, torque * torqueMultiplier, 0);
        }
        else
        {
            this.transform.Rotate(0, -baseSpeed * Time.deltaTime, 0);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            isWithinRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            isWithinRange = false;
        }
    }
}
