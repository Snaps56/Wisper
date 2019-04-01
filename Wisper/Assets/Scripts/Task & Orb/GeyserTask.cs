using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserTask : MonoBehaviour {

    public ParticleSystem waterParticles;
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

	// Use this for initialization
	void Start () {
        waterEmission = waterParticles.emission;
        waterVelocity = waterParticles.velocityOverLifetime;

        currentVelocity = waterVelocity.y.constantMax;
        reducedVelocity = currentVelocity * reducedVelocityMultiplier;
        waterVelocityCurve = new ParticleSystem.MinMaxCurve(currentVelocity, currentVelocity);

        currentEmission = waterEmission.rateOverTime.constant;
        reducedEmission = currentEmission * reducedEmissionMultiplier;
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
            geyserIsBlocked = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Geyser Trigger")
        {
            geyserIsBlocked = true;
        }
    }
}
