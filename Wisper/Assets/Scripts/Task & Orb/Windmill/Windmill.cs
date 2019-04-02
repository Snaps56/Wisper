using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {

    public GameObject abilitiesCollider;
    public float torqueMultiplier = 5f;
    public float baseSpeed = 10f;
    public float dangerSpeed = 6f;
    public float correctSpeed = 2f;
    public GameObject[] windmillParts;

    private int attachCount = 0;
    private readonly int totalToFix = 2;

    private Rigidbody rb;

    private bool reachedMaxOrbs = false;
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
        if (attachCount == totalToFix)
        {
            rb.isKinematic = false;
            Debug.Log("Windmill fixed. Need to push!");

            //If the PSD Variable WindmillFixed isn't set to true, set it to true
            if ((bool)persistantStateData.stateConditions["WindmillFixed"] == false)
            {
                persistantStateData.ChangeStateConditions("WindmillFixed", true);
            }
            //Check if the player is trying to move the windmill
            if ((isLifting || isThrowing) && isWithinRange)
            {
                Debug.Log("Player is attempting to push fixed windmill!");
                //Set the torque to move the windmill
                torque = abilitiesCollider.GetComponent<ObjectThrow>().GetThrowForce();
            }
            //If the player isn't trying to move the windmill and the task isn't done, set torque to 0
            else if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false)
            {
                Debug.Log("Player gave up pushing fixed windmill!");
                torque = 0;
            }
            currentVelocity = rb.angularVelocity.y;
            //Debug.Log("current velocity: " + currentVelocity + ", danger speed: " + dangerSpeed);

            
            if (currentVelocity >= dangerSpeed && reachedMaxOrbs)
            {
                persistantStateData.ChangeStateConditions("WindmillFixed", false);
                persistantStateData.ChangeStateConditions("WindmillTaskDone", false);

                Debug.Log("Player pushed windmill too fast and broke it");
                reachedMaxOrbs = true;
                for (int i = 0; i < windmillParts.Length; i++)
                {
                    Vector3 currentShellsterVelocity = windmillParts[i].GetComponent<Rigidbody>().velocity;
                    windmillParts[i].GetComponent<Rigidbody>().isKinematic = false;
                    windmillParts[i].transform.parent = null;
                    windmillParts[i].GetComponent<Rigidbody>().AddExplosionForce(150, transform.position, 5f);
                    Debug.Log("detached " + windmillParts[i]);
                }
            }
            else if (currentVelocity >= correctSpeed && !reachedMaxOrbs && !hasSpawnedOrbs)
            {
                Debug.Log("Player pushed windmill at correct speed and finished task.");
                hasSpawnedOrbs = true;
                GetComponent<SpawnOrbs>().DropOrbs();
                persistantStateData.ChangeStateConditions("WindmillTaskDone",true);
                //Debug.Log("WindmillTaskDone: " + (bool)persistantStateData.stateConditions["WindmillTaskDone"]);
            }
        }
    }
    private void FixedUpdate()
    {
        if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false)
        {
            rb.AddTorque(-torque * torqueMultiplier, 0, 0);
        }
        else
        {
            Debug.Log("Windmill task is done. Add passive rotation to it.");
            transform.Rotate(-baseSpeed * Time.deltaTime, 0, 0);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            isWithinRange = true;
            Debug.Log("Within Range");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            isWithinRange = false;
            Debug.Log("Exited Range");
        }
    }

    public void IncrementAttachCounter()
    {
        attachCount++;
    }
}
