using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSwirlMoving : MonoBehaviour {

    private PlayerMovement movementScript;
    private ParticleSystem mainParticleSystem;
    private ParticleSystem.VelocityOverLifetimeModule psVelocityLifetime;

    private AnimationCurve zAnimationCurve;
    private ParticleSystem.MinMaxCurve zVelocityCurve;
    private AnimationCurve yAnimationCurve;
    private ParticleSystem.MinMaxCurve yVelocityCurve;


    private float playerSpeed;

	// Use this for initialization
	void Start () {
        movementScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        mainParticleSystem = GetComponent<ParticleSystem>();
        psVelocityLifetime = mainParticleSystem.velocityOverLifetime;
        zAnimationCurve = psVelocityLifetime.z.curve;
        zVelocityCurve = new ParticleSystem.MinMaxCurve(1, zAnimationCurve);
        yAnimationCurve = psVelocityLifetime.y.curve;
        yVelocityCurve = new ParticleSystem.MinMaxCurve(1, yAnimationCurve);
    }
	
	// Update is called once per frame
	void Update () {
        playerSpeed = movementScript.GetVelocity().magnitude;
        zVelocityCurve = new ParticleSystem.MinMaxCurve(playerSpeed, zAnimationCurve);
        psVelocityLifetime.z = zVelocityCurve;

        yVelocityCurve = new ParticleSystem.MinMaxCurve(playerSpeed * 0.3f, yAnimationCurve);
        psVelocityLifetime.y = yVelocityCurve;
    }
}
