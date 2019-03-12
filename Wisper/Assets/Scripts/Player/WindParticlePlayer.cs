using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindParticlePlayer : MonoBehaviour {

    private PlayerMovement playerMovementScript;
    private ParticleSystem windParticleSystem;
    private ParticleSystem.MainModule mainPS;
    private ParticleSystem.EmissionModule emitPS;
    private ParticleSystem.VelocityOverLifetimeModule velPS;
    private ParticleSystem.ShapeModule shapePS;
    private float playerSpeed;

    private Vector3 orbitalVelocity;
    private float currentVelMultiplier = 0;

    private float shapeScale;
    private float minShapeScale;
    private float maxShapeScale;

    private float emissionRate;
    private float maxEmissionRate;
    private float minEmissionRate;

    // Use this for initialization
    void Start () {
        playerMovementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        windParticleSystem = GetComponent<ParticleSystem>();
        mainPS = windParticleSystem.main;
        emitPS = windParticleSystem.emission;
        velPS = windParticleSystem.velocityOverLifetime;
        shapePS = windParticleSystem.shape;

        shapeScale = shapePS.scale.x;
        maxShapeScale = shapeScale;
        minShapeScale = shapeScale / 2;

        emissionRate = emitPS.rateOverTime.constant;
        maxEmissionRate = emissionRate;
        minEmissionRate = emissionRate / 2;

        orbitalVelocity = new Vector3(velPS.orbitalX.constant, velPS.orbitalY.constant, velPS.orbitalZ.constant);
        //Debug.Log(orbitalVelocity);
    }
	
	// Update is called once per frame
	void Update () {
        
        playerSpeed = playerMovementScript.GetVelocity().magnitude;

        if (playerSpeed > 3)
        {
            if (currentVelMultiplier > 0)
            {
                currentVelMultiplier -= 0.02f;
            }
            if (emissionRate > minEmissionRate)
            {
                emissionRate -= 1f;
            }
            if (shapeScale > minShapeScale)
            {
                shapeScale -= 0.05f;
            }
        }
        else
        {
            if (currentVelMultiplier < 1)
            {
                currentVelMultiplier += 0.02f;
            }
            if (emissionRate < maxEmissionRate)
            {
                emissionRate += 1f;
            }
            if (shapeScale < maxShapeScale)
            {
                shapeScale += 0.05f;
            }
        }
        emitPS.rateOverTime = emissionRate;
        shapePS.scale = new Vector3(shapeScale, shapeScale, shapeScale);
        
        velPS.orbitalX = orbitalVelocity.x * currentVelMultiplier;
        velPS.orbitalY = orbitalVelocity.y * currentVelMultiplier;
        velPS.orbitalZ = orbitalVelocity.z * currentVelMultiplier;
    }
}
