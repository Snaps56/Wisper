using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour {
    
    /*****Purpose of this script *****/
    // This script was created to handle the different states of the windmill (Broken, Fixed, and Done)


    private GameObject abilitiesCollider;
    public float torqueMultiplier = 5f;
    public float baseSpeed = 10f;
    public float dangerSpeed = 6f;
    public float correctSpeed = 2f;
    public GameObject[] windmillParts;

    public GameObject fixedWing3;
    public GameObject fixedWing5;

    public GameObject brokenWing3;
    public GameObject brokenWing5;

    //Spawn location for windmill parts
    public GameObject partRespawn;

    public AudioSource audioSource;
    public AudioClip[] audioClips;

    private int attachCount = 0;
    private readonly int totalToFix = 2;

    private Rigidbody rb;

    private bool isThrowing;
    private bool isLifting;

    private float torque;
    private bool isWithinRange = false;
    private bool hasSpawnedOrbs = false;
    private float currentVelocity;
    private float maxVelocity = 1.5f;
    private PersistantStateData persistantStateData;


    // Use this for initialization
    void Start () {
        abilitiesCollider = PlayerPersistance.player.transform.Find("Abilities Collider").gameObject;
        persistantStateData = PersistantStateData.persistantStateData;
        rb = GetComponent<Rigidbody>();
        hasSpawnedOrbs = (bool)persistantStateData.stateConditions["WindmillSpawnedOrbs"];
        audioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {
        isLifting = abilitiesCollider.GetComponent<ObjectLift>().GetIsLiftingObjects();
        isThrowing = abilitiesCollider.GetComponent<ObjectThrow>().GetIsThrowingObjects();

        //Debug.Log("current velocity: " + -currentVelocity + ", correct speed: " + correctSpeed);

        if (-currentVelocity > 0.00f)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
            }
        }

        //Windmill is fixed!
        if (attachCount == totalToFix)
        {
            rb.isKinematic = false;
            //Debug.Log("Windmill fixed. Need to push!");

            //PSD WindmillFixed set to true
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
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
                //}
                //Debug.Log("Sound should play");
            }
            else if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false) // Otherwise, reset torque
            {
                //Debug.Log("Player gave up pushing fixed windmill!");
                torque = 0;
            }
            currentVelocity = rb.angularVelocity.y;
            //Debug.Log("HasReachedMax: " + (bool)persistantStateData.stateConditions["HasReachedMax"]);

            //Windmill is Broken!
            if ((bool)persistantStateData.stateConditions["HasReachedMax"] == true && Input.GetKeyDown(KeyCode.K)) //Based on button press for testing purposes
            {
                //rb.isKinematic = true;
                attachCount = 0;
                persistantStateData.ChangeStateConditions("WindmillFixed", false);
                persistantStateData.ChangeStateConditions("WindmillTaskDone", false);
                fixedWing3.SetActive(false);
                fixedWing5.SetActive(false);
                brokenWing3.SetActive(true);
                brokenWing5.SetActive(true);

                Instantiate(windmillParts[0], partRespawn.transform.position, Quaternion.identity);
                Instantiate(windmillParts[1], partRespawn.transform.position, Quaternion.identity);
                //Debug.Log("Player pushed windmill too fast and broke it");
                for (int i = 0; i < windmillParts.Length; i++)
                {
                    windmillParts[i].GetComponent<Rigidbody>().AddExplosionForce(150, windmillParts[i].transform.position, 5f);
                    //Debug.Log("detached " + windmillParts[i]);
                }
            }
            //Windmill task is done! 
            else if (-currentVelocity >= correctSpeed)
            {
                //Debug.Log("Player pushed windmill at correct speed and finished task.");
                //Check if the player spawned the orbs
                if (!hasSpawnedOrbs)
                {
                    persistantStateData.ChangeStateConditions("WindmillSpawnedOrbs", true);
                    this.gameObject.GetComponent<SpawnOrbs>().DropOrbs();
                    hasSpawnedOrbs = true;
                }
                persistantStateData.ChangeStateConditions("WindmillTaskDone",true);
                //Debug.Log("WindmillTaskDone: " + (bool)persistantStateData.stateConditions["WindmillTaskDone"]);
            }
        }
        else
        {
            torque = 0;
        }
    }
    private void FixedUpdate()
    {
        //If the taks isn't done, allow the user to move the windmill
        if ((bool)persistantStateData.stateConditions["WindmillTaskDone"] == false)
        {
            //Debug.Log("Turning Windmill.");
            rb.AddTorque(-torque * torqueMultiplier, 0, 0);
        }
        else //If task is done, add constant passive force
        {
            //Debug.Log("Windmill task is done. Add passive rotation to it.");
            if (-currentVelocity < maxVelocity)
            {
                rb.isKinematic = false;
                //Debug.Log("Turning blah blah blah");
                rb.AddTorque(-baseSpeed, 0, 0);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check if the player collided
        if (other.name == "Abilities Collider")
        {
            isWithinRange = true;
            //Debug.Log("Within Range");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Check if the player left
        if (other.name == "Abilities Collider")
        {
            isWithinRange = false;
            //Debug.Log("Exited Range");
        }
    }
    //Increment the attach counter of the windmill
    public void IncrementAttachCounter()
    {
        attachCount++;
    }
}
