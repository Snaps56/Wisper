using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerryGoRound : MonoBehaviour {

    public GameObject abilitiesCollider;
    public float torqueMultiplier = 0.05f;
    public float dangerSpeed = 6f;
    public float correctSpeed = 4f;
    public GameObject[] shellsters;

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
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        rb = GetComponent<Rigidbody>();
        abilitiesCollider = PlayerPersistance.player.transform.Find("Abilities Collider").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        isLifting = abilitiesCollider.GetComponent<ObjectLift>().GetIsLiftingObjects();
        isThrowing = abilitiesCollider.GetComponent<ObjectThrow>().GetIsThrowingObjects();

        if ((isLifting || isThrowing) && isWithinRange)
        {
            torque = abilitiesCollider.GetComponent<ObjectThrow>().GetThrowForce();
        }
        else
        {
            torque = 0;
        }
        currentVelocity = rb.angularVelocity.y;
        //Debug.Log("current velocity: " + currentVelocity + ", danger speed: " + dangerSpeed);

        if (currentVelocity >= dangerSpeed && !reachedDangerSpeed)
        {
            reachedDangerSpeed = true;
            for (int i = 0; i < shellsters.Length; i++)
            {
                Vector3 currentShellsterVelocity = shellsters[i].GetComponent<Rigidbody>().velocity;
                shellsters[i].GetComponent<Rigidbody>().isKinematic = false;
                shellsters[i].transform.parent = null;
                shellsters[i].GetComponent<Rigidbody>().AddExplosionForce(150, transform.position, 5f);
                Debug.Log("detached " + shellsters[i]);
            }
        }
        else if (currentVelocity >= correctSpeed && !reachedDangerSpeed && !hasSpawnedOrbs) {
            /*
            hasSpawnedOrbs = true;
            GetComponent<SpawnOrbs>().DropOrbs();
            persistantStateData.stateConditions["MerryGoRound"] = true;
            */
        }
	}
    private void FixedUpdate()
    {
        rb.AddTorque(0, torque * torqueMultiplier, 0);
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
