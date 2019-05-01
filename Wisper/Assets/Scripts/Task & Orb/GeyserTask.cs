using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserTask : MonoBehaviour {

    public ParticleSystem waterParticles;

    public Animator [] geyserShellstersAnim;
    public AudioSource geyserWaterLoopSound;
    public AudioSource boulderRollingSound;

    private bool isGettingPickedUp = false;

    private Transform boulderOffset;
    private ParticleSystem.EmissionModule waterEmission;
    private ParticleSystem.VelocityOverLifetimeModule waterVelocity;

    private ParticleSystem.MinMaxCurve waterVelocityCurve;

    private float currentVelocity;
    private float reducedVelocity;
    private float reducedVelocityMultiplier = 0f;

    private float currentEmission;
    private float reducedEmission;
    private float reducedEmissionMultiplier = 0f;

    private Rigidbody rb;
    private bool geyserIsBlocked = false;

    private SpawnOrbs spawnOrbsScript;

	// Use this for initialization
	void Start () {
        waterEmission = waterParticles.emission;
        waterVelocity = waterParticles.velocityOverLifetime;

        currentVelocity = waterVelocity.y.constantMax;
        reducedVelocity = currentVelocity * reducedVelocityMultiplier;
        waterVelocityCurve = new ParticleSystem.MinMaxCurve(currentVelocity, currentVelocity);

        currentEmission = waterEmission.rateOverTime.constant;
        reducedEmission = currentEmission * reducedEmissionMultiplier;

        spawnOrbsScript = GetComponent<SpawnOrbs>();

        boulderOffset = GameObject.Find("BoulderOffset").transform;

        if ((bool)PersistantStateData.persistantStateData.stateConditions["GeyserTask"])
        {
            transform.position = boulderOffset.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("Geyser blocked? " + geyserIsBlocked);
        if (geyserIsBlocked)
        {
            waterEmission.rateOverTime = reducedEmission;
            waterVelocityCurve = new ParticleSystem.MinMaxCurve(reducedVelocity, reducedVelocity);
            waterVelocity.y = waterVelocityCurve;
        }
        else
        {
            waterEmission.rateOverTime = currentEmission;
            waterVelocityCurve = new ParticleSystem.MinMaxCurve(currentVelocity, currentVelocity);
            waterVelocity.y = waterVelocityCurve;
        }
	}
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Geyser Trigger")
        {
            if (!(bool)PersistantStateData.persistantStateData.stateConditions["GeyserTask"])
            {
                for (int i = 0; i <  geyserShellstersAnim.Length; i++)
                {
                    geyserShellstersAnim[i].SetTrigger("GeyserSpraying");
                }
                spawnOrbsScript.DropOrbs();
                PersistantStateData.persistantStateData.ChangeStateConditions("GeyserTask", true);
            }
            geyserIsBlocked = false;
            geyserWaterLoopSound.Play();
        }
        if (other.gameObject.tag == "Terrain")
        {
            if (boulderRollingSound.isPlaying)
            {
                boulderRollingSound.Stop();
            }
        }
        if (other.gameObject.name == "Abilities Collider")
        {
            isGettingPickedUp = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Boulder collided with " + other.name);
        if (other.gameObject.name == "Geyser Trigger")
        {
            if (!boulderRollingSound.isPlaying)
            {
                if (isGettingPickedUp)
                {
                    Debug.Log("Play Rolling Sound!");
                    
                    boulderRollingSound.Play();
                }
            }
        }
        if (other.gameObject.name == "Abilities Collider")
        {
            Debug.Log("Boulder within player collider! Getting Picked UP? " + isGettingPickedUp);
            if (other.gameObject.GetComponent<ObjectThrow>().GetIsThrowingObjects() || other.gameObject.GetComponent<ObjectLift>().GetIsLiftingObjects())
            {
                isGettingPickedUp = true;
            }
            else
            {
                if (boulderRollingSound.isPlaying)
                {
                    boulderRollingSound.Stop();
                }
                isGettingPickedUp = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Geyser Trigger")
        {
            geyserIsBlocked = true;
            geyserWaterLoopSound.Stop();
        }
    }
}
