using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FlarePulse : MonoBehaviour
{
    public LensFlare lensFlare;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;

    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;



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
            //Debug.Log("Is pulsing");
            //Begins the timer
            BeginTimer();
            GamePad.SetVibration(playerIndex, 0.01f , 0.1f);

            //Reassigns the brightness level
            lensFlare.brightness = currentBrightness;
        }

        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    //Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        prevState = state;
        state = GamePad.GetState(playerIndex);

    }
}
