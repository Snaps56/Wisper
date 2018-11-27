using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFader : MonoBehaviour {

    [Header("GameObjects")]
    public GameObject dialogueBox;

    [Header("Properties")]

    public float dialogueOffset;

    public float delayTime;
    public float fadeInDuration; // how long it takes for tutorial prompts to fade away
    public float fadeOutDuration; // how long it takes for tutorial prompts to fade away
    public float maxDuration; // how long tutorial stays up without player action
    public float minDuration; // minimum time tutorial stays up before fading

    private TutorialChecker tutorialCheckerScript;

    private float initialDelayCounter = 0;
    private bool delayTimeAssigned = false;
    private float delayCounter = 0;
    private bool doneDelay = false;

    private float durationCounter = 0;
    private float initialTime;
    private bool obtainedInitialTime = false;

    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private bool doneFading = false;

    private float alphaValue = 0f;
    private float deltaAlphaInValue;
    private float deltaAlphaOutValue;

    private bool dialogueEnabled;
    private Vector3 originalPosition;
    private Vector3 dialoguePosition;
    
    private bool updatedPSD = false;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;

        deltaAlphaInValue = 1 / fadeInDuration;
        deltaAlphaOutValue = 1 / fadeOutDuration;
        GetComponent<CanvasGroup>().alpha = alphaValue;

        tutorialCheckerScript = GetComponent<TutorialChecker>();
    }
	
	// Update is called once per frame
	void Update () {

        DialogueOffset();
        DelayTutorial();

        if (doneDelay)
        {
            if (!obtainedInitialTime)
            {
                initialTime = Time.time;
                obtainedInitialTime = true;
            }
            
            durationCounter = Time.time - initialTime;

            FadeIn();
            FadeOut();
        }
        if (doneFading && !updatedPSD)
        {
            tutorialCheckerScript.updatePSD();
            updatedPSD = true;
        }
    }
    void DelayTutorial()
    {
        if (tutorialCheckerScript.InitialConditionsMet())
        {
            if (!delayTimeAssigned)
            {
                initialDelayCounter = Time.time;
                delayTimeAssigned = true;
            }
            delayCounter = Time.time - initialDelayCounter;
        }

        if (delayCounter > (initialTime + delayTime))
        {
            doneDelay = true;
            isFadingIn = true;
        }

    }
    void FadeIn()
    {
        if (isFadingIn && !isFadingOut && alphaValue < 1)
        {
            alphaValue += deltaAlphaInValue * Time.deltaTime;
            GetComponent<CanvasGroup>().alpha = alphaValue;
        }
        else
        {
            isFadingIn = false;
        }

    }
    void FadeOut()
    {
        bool durationCondition = false;
        if (durationCounter > maxDuration)
        {
            durationCondition = true;
        }
        
        bool minDurationCondition = false;
        if (durationCounter > minDuration)
        {
            minDurationCondition = true;
        }
        
        if (!isFadingIn && (durationCondition || (tutorialCheckerScript.TutorialConditionMet() && minDurationCondition)))
        {
            isFadingOut = true;
        }

        if (isFadingOut && alphaValue > 0)
        {
            alphaValue -= deltaAlphaOutValue * Time.deltaTime;
            GetComponent<CanvasGroup>().alpha = alphaValue;
        }

        if (doneDelay && !isFadingIn && isFadingOut && (alphaValue <= 0))
        {
            doneFading = true;
        }
    }
    void DialogueOffset()
    {
        if (dialogueBox.activeSelf)
        {
            transform.position = originalPosition + new Vector3(0, dialogueOffset, 0);
        }
        else
        {
            transform.position = originalPosition;
        }
    }
}
