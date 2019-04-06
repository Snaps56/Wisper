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
    
    // variables that control alpha values of graphics
    private float alphaValue = 0f;
    private float deltaAlphaInValue;
    private float deltaAlphaOutValue;

    // 
    private bool dialogueEnabled;
    private Vector3 originalPosition;
    private Vector3 dialoguePosition;
    
    private bool updatedPSD = false;
    private bool psdInitial;

    // Use this for initialization
    void Start () {
        originalPosition = transform.position;

        deltaAlphaInValue = 1 / fadeInDuration;
        deltaAlphaOutValue = 1 / fadeOutDuration;
        GetComponent<CanvasGroup>().alpha = alphaValue;

        tutorialCheckerScript = GetComponent<TutorialChecker>();
        psdInitial = tutorialCheckerScript.GetSceneChecker();
    }
	
	// Update is called once per frame
	void Update () {

        // if this class has not been initialized before, begin all operations
        if (!psdInitial)
        {
            // check if tutorials need to be moved
            DialogueOffset();
            DelayTutorial();

            // if the delay timer is done, begin fade timers
            if (doneDelay)
            {
                if (!obtainedInitialTime)
                {
                    initialTime = Time.time;
                    obtainedInitialTime = true;
                }

                durationCounter = Time.time - initialTime;

                // call fade functions which will handle fade in and fade out
                FadeIn();
                FadeOut();
            }

            // if the fading out is finished, update the Persistant State Data variables
            if (doneFading && !updatedPSD)
            {
                tutorialCheckerScript.updatePSD();
                updatedPSD = true;
            }
        }
    }

    // delay the tutorial using a timer, begin fading only after this timer is done
    void DelayTutorial()
    {
        // if initial conditions have been met, trigger the delay timer
        if (tutorialCheckerScript.InitialConditionsMet())
        {
            if (!delayTimeAssigned)
            {
                initialDelayCounter = Time.time;
                delayTimeAssigned = true;
            }
            delayCounter = Time.time - initialDelayCounter;
        }
        // if delay time has been completed, stop the delay and trigger the fade in
        if (delayCounter > (initialTime + delayTime))
        {
            doneDelay = true;
            isFadingIn = true;
        }

    }

    // tutorial fade-in using a timer
    void FadeIn()
    {
        // do graphic fade in only when the graphic is not fading out and has not completed
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

    // tutorial fading-out only if fading-in timer is done
    void FadeOut()
    {
        // check if graphic tutorial has run its duration before fading out
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
        
        // if all necessary conditions are met, begin fading out
        if (!isFadingIn && (durationCondition || (tutorialCheckerScript.TutorialConditionMet() && minDurationCondition)))
        {
            isFadingOut = true;
        }

        // assuming we can begin fading out, reduce alpha value of tutorial over time
        if (isFadingOut && alphaValue > 0)
        {
            alphaValue -= deltaAlphaOutValue * Time.deltaTime;
            GetComponent<CanvasGroup>().alpha = alphaValue;
        }

        // change a bool if the tutorial is done fading out
        if (doneDelay && !isFadingIn && isFadingOut && (alphaValue <= 0))
        {
            doneFading = true;
        }
    }

    // tutorial is pushed upward if the dialogue box is active, otherwise return to original position
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
