using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Marker : MonoBehaviour {
    
    // Gameobjects to assign in the inspector
    [Header("GameObjects")]
    private Camera mainCamera;
    private Transform character;
    private Canvas mainCanvas;
    public GameObject indicator;

    // Variables pertaining to positioning and alpha values
    [Header("Values")]
    public float farYOffset; // offset the marker in the y direction
    public float closeYOffset; // move marker offset as player gets closer so that icon sits nicely over the interactable
    private float finalYOffset;

    public float minAlphaDistance; // distance where marker will have its maximum alpha value
    public float maxDrawDistance; // distance where marker will have its min alpha value
    public float defaultAlpha; // alpha value when icon is interactable

    // alpha value scales between these two values based on distance
    public float highAlpha; // high alpha value at minAlphaDistance
    public float lowAlpha; // lowest alpha value at maxDrawDistance

    private float deltaAlpha; // alpha value change per distance unit
    private float currentAlpha; // current alpha value of icon
    private float distance; // current distance of player
    private float deltaDistance; // distance between minAlphaDistance and maxAlphaDistance
    private GameObject waypoint;

    // variables pertaining to the shaking animation of icon
    [Header("Animation")]
    public float shakeDelay; // delay between each shake
    public float shakeMax; // maximum shake value
    public float shakeDuration; // duration of shake before stopping
    private float currentShake; // current value of shake, capped at shakeMax

    private bool isAnimating; // boolean variables that determine if icon is currently shaking
    private bool doneAnimating;

    private float linearInterpolate = 0f; // interpolation used to shake the icon based on timer
    private float initialTimer; // initial time based on when icon first appears
    private float currentTimer;

    // variables relating to conditions that change how the icon behaves
    [Header("Persistant State Data")]
    public string[] dependentConditions; // conditions in persistant state data are required to trigger the Marker
    private bool dependentConditionsMet;

    public string[] onInteractConditions; // conditions in persistant state data are changed after initial interaction
    public string[] disableMarkerConditions;// conditions in persistant state data that disable the marker
    private bool disableMarker; // disable the marker if any disable marker conditions are true;

    private PersistantStateData persistantStateData;
    
	// Use this for initialization
	void Start ()
    {
        // find required game objects
        character = GameObject.Find("Player").transform;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        // create waypoint icon
        waypoint = Instantiate(indicator);
        waypoint.transform.SetParent(mainCanvas.GetComponent<RectTransform>());
        waypoint.transform.SetAsFirstSibling();
        waypoint.GetComponent<RectTransform>().SetParent(mainCanvas.transform);
        waypoint.SetActive(false);

        // initialize alpha settings
        deltaDistance = maxDrawDistance - minAlphaDistance;
        deltaAlpha = (highAlpha - lowAlpha) / (deltaDistance);

        // initialize timer variables
        initialTimer = Time.time;
        isAnimating = false;
        doneAnimating = false;


        // initialize condition variables
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        dependentConditionsMet = false;
    }
	
    // shake icon only when shake cooldown is done
    void IconShake()
    {
        // increase shake until it hits max shake value
        if (currentShake < shakeMax && !doneAnimating)
        {
            if (linearInterpolate < 1)
            {
                linearInterpolate += (2 / shakeDuration) * Time.deltaTime;
            }
            currentShake = Mathf.Lerp(1, shakeMax, linearInterpolate);
        }
        else
        {
            // otherwise decrease shake until it hits minimum shake value
            doneAnimating = true;
            if (linearInterpolate > 0)
            {
                linearInterpolate -= (2 / shakeDuration) * Time.deltaTime;
            }
            currentShake = Mathf.Lerp(1, shakeMax, linearInterpolate);
        }

        // set shake value to minimum when animation is done
        if (linearInterpolate <= 0 && doneAnimating)
        {
            linearInterpolate = 0f;
            currentShake = Mathf.Lerp(1, shakeMax, linearInterpolate);
            isAnimating = false;
        }

    }
    void BeginTimer()
    {
        // obtain cooldown time of shake after initial shake
        currentTimer = Time.time - initialTimer;

        // reset cooldown of shake after delay timer is met
        if (currentTimer > shakeDelay)
        {
            doneAnimating = false;
            isAnimating = true;
            initialTimer = Time.time;
        }

        // shake icon only when shake cooldown is done
        if (isAnimating)
        {
            IconShake();
        }

    }
    // Update is called once per frame
    void Update () {
        // check if dependent conditions have been met to render icon
        if (!dependentConditionsMet)
        {

            // iterate through all dependent conditions to check if they are being met
            bool allConditionsTrue = true;
            if (dependentConditions != null)
            {
                for (int i = 0; i < dependentConditions.Length; i++)
                {
                    bool currentCondition = ((bool)persistantStateData.stateConditions[dependentConditions[i]]);
                    if (!currentCondition)
                    {
                        allConditionsTrue = false;
                    }
                }
            }
            else
            {
                // all conditions have been met, so escape through parent IF statement and go to ELSE
                allConditionsTrue = true;
            }

            if (allConditionsTrue)
            {
                dependentConditionsMet = true;
            }
        }
        else
        {
            // obtain distance between parent object and the player
            distance = Vector3.Distance(character.position, transform.position);

            // obtain direction of the marker relative to camera
            Vector3 rayCastDirection = (transform.position - mainCamera.transform.position).normalized;
            float rayCastCheck = Vector3.Dot(rayCastDirection, mainCamera.transform.forward);

            // check if marker is also drawing behind player, if so, don't draw the marker that is behind
            if (rayCastCheck <= 0f || distance >= maxDrawDistance)
            {
                waypoint.SetActive(false);
            }
            else
            {
                // check if there are any conditions that disable the marker icon
                if (disableMarkerConditions != null)
                {
                    for (int i = 0; i < disableMarkerConditions.Length; i++)
                    {
                        bool currentCondition = ((bool)persistantStateData.stateConditions[disableMarkerConditions[i]]);
                        if (currentCondition)
                        {
                            disableMarker = true;
                        }
                    }
                }
                else
                {
                    disableMarker = false;
                }

                // draw front marker only when marker is not disabled based on disable marker conditions
                if (!disableMarker)
                {
                    // draw marker at parent position along with the offset y position
                    Vector3 offsetPosition = transform.position;

                    float distanceInterpolation = distance / maxDrawDistance;
                    finalYOffset = Mathf.Lerp(closeYOffset, farYOffset, distanceInterpolation);
                    offsetPosition.y = transform.position.y + finalYOffset;

                    // draw marker onto the canvas at same relative location as target
                    waypoint.GetComponent<RectTransform>().position = mainCamera.WorldToScreenPoint(offsetPosition);
                    waypoint.SetActive(true);

                    // modify alpha values of icon
                    UIAlpha();
                }
                else
                {
                    waypoint.SetActive(false);
                }
            }
        }

    }
    void UIAlpha()
    {
        // alpha scaling with distance
        if (distance > minAlphaDistance)
        {
            // timer that shakes the icon
            BeginTimer();
            waypoint.transform.localScale = new Vector3(currentShake, currentShake, currentShake);

            float distancePercent = deltaDistance - (distance - minAlphaDistance);

            // set alpha value of icon
            currentAlpha = distancePercent * deltaAlpha + lowAlpha;
            waypoint.GetComponent<CanvasGroup>().alpha = currentAlpha;
        }
        else
        {
            // if player is within interactable distance
            // draw icon at max alpha value and at max shake
            waypoint.transform.localScale = new Vector3(shakeMax, shakeMax, shakeMax);
            waypoint.GetComponent<CanvasGroup>().alpha = defaultAlpha;

            // change conditions if player interacts with object at interactable range
            if (Input.GetButton("PC_Key_Interact") || Input.GetButton("XBOX_Button_A"))
            {
                for (int i = 0; i < onInteractConditions.Length; i++)
                {
                    persistantStateData.ChangeStateConditions(onInteractConditions[i], true);
                }
            }

        }
    }
}
