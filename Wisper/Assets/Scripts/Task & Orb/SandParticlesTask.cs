using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandParticlesTask : MonoBehaviour {

    private Rigidbody rb;
    private ParticleSystem sandParticleSystem;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.MainModule mainModule;
    private float initialEmission;
    private float initialLifetime;
    private float newLifetime = 99999999999;

    private bool isThrowing;
    private bool isLifting;

    private bool modifiedStationaryParticles = false;
    private ParticleSystem.Particle[] currentParticles;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        sandParticleSystem = GetComponent<ParticleSystem>();
        emissionModule = sandParticleSystem.emission;
        mainModule = sandParticleSystem.main;
        initialEmission = emissionModule.rateOverTime.constant;
        initialLifetime = mainModule.startLifetime.constant;

        isThrowing = GameObject.Find("Player").GetComponent<ObjectThrow>().GetIsThrowingObjects();
        isLifting = GameObject.Find("Player").GetComponent<ObjectLift>().GetIsLiftingObjects();
        currentParticles = new ParticleSystem.Particle[sandParticleSystem.main.maxParticles];
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(rb.velocity.magnitude);
		if (rb.velocity.magnitude > 0.1f)
        {
            emissionModule.rateOverTime = initialEmission;
            mainModule.startLifetime = initialLifetime;
            modifiedStationaryParticles = false;
        }
        else
        {
            if (!modifiedStationaryParticles)
            {
                currentParticles = new ParticleSystem.Particle[sandParticleSystem.GetParticles(currentParticles)];
                for (int i = 0; i < currentParticles.Length; i++)
                {
                    currentParticles[i].remainingLifetime = newLifetime;
                }
                sandParticleSystem.SetParticles(currentParticles, currentParticles.Length - 1);
                emissionModule.rateOverTime = 0;
                modifiedStationaryParticles = true;
            }
        }
	}
}
