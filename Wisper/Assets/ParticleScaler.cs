using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour {

    public GameObject player;
    public float emissionIncreaseModifier;
    public float velocityIncreaseModifier;
    public float lifetimeVelocityIncreaseModifier;

    private ParticleSystem ps;
    private ParticleSystem.MainModule mainModule;
    private ParticleSystem.EmissionModule emissionModule;
    private ParticleSystem.VelocityOverLifetimeModule velocityLifetimeModule;
    private float initialEmissionRate;
    private float finalEmissionRate;

    private float numOrbs;
    
    private float initialVelocity;
    private float finalVelocity;

    private float initialLifetimeVelocity;
    private float finalLifetimeVelocity;

    // Use this for initialization
    void Start () {
        numOrbs = player.GetComponent<OrbCount>().GetOrbCount();

        ps = GetComponent<ParticleSystem>();
        emissionModule = ps.emission;
        velocityLifetimeModule = ps.velocityOverLifetime;

        initialEmissionRate = emissionModule.rateOverTime.constant;
        initialVelocity = ps.main.startSpeed.constant;
        initialLifetimeVelocity = velocityLifetimeModule.speedModifier.constant;
    }
	
	// Update is called once per frame
	void Update ()
    {
        numOrbs = player.GetComponent<OrbCount>().GetOrbCount();

        finalEmissionRate = initialEmissionRate + numOrbs * emissionIncreaseModifier;
        emissionModule = ps.emission;
        emissionModule.rateOverTime = finalEmissionRate;

        finalVelocity = initialVelocity + numOrbs * velocityIncreaseModifier;
        mainModule = ps.main;
        mainModule.startSpeed = finalVelocity;

        finalLifetimeVelocity = initialLifetimeVelocity + numOrbs * lifetimeVelocityIncreaseModifier;
        velocityLifetimeModule = ps.velocityOverLifetime;
        velocityLifetimeModule.speedModifier = finalLifetimeVelocity;
    }
}
