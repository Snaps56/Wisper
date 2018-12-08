using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlarePulse : MonoBehaviour
{
    public LensFlare lensFlare;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;



    //The camera's forward vector
    private Vector3 cameraForward;
    //The vector between the shrine and camera
    private Vector3 toFlare;

    private float linearInterpolate = 0f;

    private float initialTimer;
    private float currentTimer;

    private bool isPulsing;
    private bool donePulsing;
    public float pulseDelay;
    public float maxBrightness;
    public float pulseDuration;
    private float currentBrightness;
    private float distance;
    private float initialBrigheness;

    // Use this for initialization
    void Start()
    {
        initialTimer = Time.time;
        isPulsing = false;
        donePulsing = false;
    }

    void BeginTimer()
    {
        // Debug.Log("Cooldown: " + currentTimer + ", current Shake: " + currentShake);
        currentTimer = Time.time - initialTimer;

        if (currentTimer > pulseDelay)
        {
            donePulsing = false;
            isPulsing = true;
            initialTimer = Time.time;
        }

        if (isPulsing)
        {
            LightPulse();
        }

    }

    void LightPulse()
    {
        if (currentBrightness < maxBrightness && !donePulsing)
        {
            if (linearInterpolate < 1)
            {
                linearInterpolate += (2 / pulseDuration) * Time.deltaTime;
            }
            currentBrightness = Mathf.Lerp(initialBrigheness, maxBrightness, linearInterpolate);
        }
        else
        {
            donePulsing = true;
            if (linearInterpolate > 0)
            {
                linearInterpolate -= (2 / pulseDuration) * Time.deltaTime;
            }
            currentBrightness = Mathf.Lerp(initialBrigheness, maxBrightness, linearInterpolate);
        }

        if (linearInterpolate <= 0 && donePulsing)
        {
            linearInterpolate = 0f;
            currentBrightness = Mathf.Lerp(initialBrigheness, maxBrightness, linearInterpolate);
            isPulsing = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //Update the forward vector
        cameraForward = mainCamera.transform.forward;

        //Find direction between the shrine and camera
        toFlare = lensFlare.transform.position - mainCamera.transform.position;

        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toFlare.normalized) > dotProductAngle)
        {
            Debug.Log("Is pulsing");
            //Begins the timer
            BeginTimer();

            //Reassigns the brightness level
            lensFlare.brightness = currentBrightness;
        }

    }
}
