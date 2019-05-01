using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueRotation : MonoBehaviour
{
    public Transform rotatePoint;
    public ObjectThrow playerAbilities;
    public ParticleSystem stoneDustParticles;
    public AudioSource stoneGrind;

    private bool playerPushing;
    private Vector3 playerPushVector;
    private Vector3 statueVector;
    private float finalDirection;
    private bool nearPlayer;
    private SpawnOrbs orbScript;

    //float rotation = 0f;
    Vector3 desiredRotation;
    float xOffset;
    float yOffset;
    float zOffset;
    bool taskDone;

    // Use this for initialization
    void Start()
    {
        xOffset = 0f;
        yOffset = 0f;
        playerAbilities = GameObject.Find("Abilities Collider").GetComponent<ObjectThrow>();
        statueVector = transform.right;
        nearPlayer = false;
        taskDone = (bool)PersistantStateData.persistantStateData.stateConditions["StatueFixed"];
        orbScript = GetComponent<SpawnOrbs>();
        //stoneGrind = GetComponent<AudioSource>();
        if (!taskDone) {
            transform.rotation = Quaternion.Euler(xOffset, yOffset, 30f);
        }
        Debug.Log(taskDone);
    }

    // Update is called once per frame
    void Update()
    {
        //rotation = 0f;
        //xOffset = transform.localEulerAngles.x;
        //xOffset = 0f;
        //yOffset = transform.localEulerAngles.y - 90f;
        //yOffset = -90f;
        desiredRotation = transform.localRotation.eulerAngles;
        desiredRotation.z = Mathf.Clamp(desiredRotation.z, -30f, 30f);
        //zOffset = desiredRotation.z;
        //zOffset = Mathf.Clamp(transform.localRotation.eulerAngles.z, -30f, 30f);
        //zOffset = 20 * Time.deltaTime;
        zOffset = transform.localRotation.eulerAngles.z;
        if (zOffset > 180f) {
            zOffset = zOffset - 360f;
        }
        Debug.Log("zOffset: " + zOffset);
        
        playerPushing = playerAbilities.GetIsThrowingObjects();
        playerPushVector = playerAbilities.GetMovementVector();
        //Debug.Log("Statue Vector: " + statueVector);
        //Debug.Log("Player Pushing: " + playerPushing);
        //Debug.Log("Player Direction Vector " + playerPushVector);

        finalDirection = Vector3.Dot(playerPushVector.normalized, statueVector.normalized);
        //Debug.Log("Final Direction: " + finalDirection);

        if (nearPlayer == true && playerPushing == true && taskDone == false)
        {
            if (!stoneDustParticles.isPlaying)
            {
                stoneDustParticles.Play();
            }
            if (!stoneGrind.isPlaying)
            {
                stoneGrind.Play();
            }

            if (finalDirection < 0 && zOffset <= 30f)
            {
                Debug.Log("zOffset POSDIR: " + zOffset);
                transform.RotateAround(rotatePoint.position, Vector3.forward, 20 * Time.deltaTime);
                
            }
            else if (finalDirection >= 0 && zOffset >= -30f)
            {
                Debug.Log("zOffset NEGDIR: " + zOffset);
                transform.RotateAround(rotatePoint.position, Vector3.forward, -20 * Time.deltaTime);
            }
        }
        else
        {
            if (stoneDustParticles.isPlaying)
            {
                stoneDustParticles.Stop();
            }
            if (stoneGrind.isPlaying)
            {
                stoneGrind.Stop();
            }
        }

        if (playerPushing == false && (zOffset > -3f && zOffset < 3f) && taskDone == false) {
            Debug.Log("Statue is upright");
            orbScript.DropOrbs();
            PersistantStateData.persistantStateData.ChangeStateConditions("StatueFixed", true);
            taskDone = true;
        }

        /*
        if (zOffset > 30 && zOffset < 180)
        {
            Debug.Log("Too far +!");
            zOffset = 30f;
            transform.localRotation = Quaternion.Euler(xOffset, yOffset, zOffset);
        }
        else if (zOffset < 330 && zOffset > 180)
        {
            Debug.Log("Too far -!");
            zOffset = -30f;
            transform.localRotation = Quaternion.Euler(xOffset, yOffset, zOffset);
        }
        */

        //transform.localRotation = Quaternion.Euler(xOffset, yOffset, zOffset);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            nearPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Abilities Collider")
        {
            nearPlayer = false;
        }
    }
}
