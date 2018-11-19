using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheckMovement : MonoBehaviour {

    public GameObject player;
    public GameObject dialogueBox;
    public float dialogueOffset;

    public float fadeInDuration; // how long it takes for tutorial prompts to fade away
    public float fadeOutDuration; // how long it takes for tutorial prompts to fade away
    public float maxDuration; // how long tutorial stays up without player action
    public float minDuration; // minimum time tutorial stays up before fading
    public float distanceTravelRequired;
    
    private float playerDistanceTraveled = 0;

    private float durationCounter = 0;
    private float initialTime;
    private bool obtainedInitialTime = false;
    private bool isFadingIn = true;
    private bool isFadingOut = false;

    private float alphaValue = 0f;
    private float deltaAlphaInValue;
    private float deltaAlphaOutValue;

    private bool dialogueEnabled;
    private Vector3 originalPosition;
    private Vector3 dialoguePosition;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;

        deltaAlphaInValue = 1 / fadeInDuration;
        deltaAlphaOutValue = 1 / fadeOutDuration;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (dialogueBox.activeSelf)
        {
            transform.position = originalPosition + new Vector3(0, dialogueOffset,0);
        }
        else
        {
            if (!obtainedInitialTime)
            {
                initialTime = Time.time;
                obtainedInitialTime = true;
            }

            transform.position = originalPosition;

            playerDistanceTraveled += player.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

            durationCounter = Time.time - initialTime;

            if (isFadingIn && alphaValue < 1)
            {
                alphaValue += deltaAlphaInValue * Time.deltaTime;
                GetComponent<CanvasGroup>().alpha = alphaValue;
            }
            else
            {
                isFadingIn = false;
            }

            if (!isFadingIn && ((durationCounter > maxDuration) || ((playerDistanceTraveled > distanceTravelRequired) && (durationCounter > minDuration))))
            {
                isFadingOut = true;
            }

            if (isFadingOut && alphaValue > 0)
            {
                alphaValue -= deltaAlphaOutValue * Time.deltaTime;
                GetComponent<CanvasGroup>().alpha = alphaValue;
            }
        }
	}
}
