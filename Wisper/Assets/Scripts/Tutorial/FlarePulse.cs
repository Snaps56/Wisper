using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FlarePulse : MonoBehaviour
{
    public LensFlare lensFlare;
    private Camera mainCamera;
    public float dotProductAngle = 0.9f;

    public bool VibStop = false;
    public bool StoppedVib = false;
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
        lensFlare.brightness = 5;
        initialTimer = Time.time;
        isPulsing = false;
        donePulsing = false;

        mainCamera = GameObject.Find("Player").transform.Find("Main Camera").GetComponent<Camera>();
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);

        VibStop = true;
        StoppedVib = true;
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Update the forward vector
        cameraForward = mainCamera.transform.forward;

        //Find direction between the shrine and camera
        toFlare = lensFlare.transform.position - mainCamera.transform.position;

        if ((Vector3.Dot(cameraForward.normalized, toFlare.normalized) < (dotProductAngle - 0.1f) || VibStop == true || Time.timeScale == 0) && StoppedVib == false)
        {
            GamePad.SetVibration(playerIndex, 0f, 0f);
        }
        if (!VibStop && Time.timeScale == 1f)
        {
            if (Vector3.Dot(cameraForward.normalized, toFlare.normalized) > (dotProductAngle - 0.1f))
            {
                    GamePad.SetVibration(playerIndex, 0f, 1f);
            }
            if (Vector3.Dot(cameraForward.normalized, toFlare.normalized) > (dotProductAngle))
            {
                GamePad.SetVibration(playerIndex, 0.1f, 1f);
                StartCoroutine(Wait());
            }
        }
        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toFlare.normalized) > (dotProductAngle))
        {
            //Debug.Log("Is pulsing");
            //Begins the timer
            BeginTimer();

            //Reassigns the brightness level
            lensFlare.brightness = currentBrightness;
        }
    }
}
