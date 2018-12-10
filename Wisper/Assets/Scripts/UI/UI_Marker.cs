﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Marker : MonoBehaviour {
    
    [Header("GameObjects")]
    public Camera mainCamera;
    public Transform character;
    public Canvas mainCanvas;
    public GameObject indicator;

    [Header("Values")]
    public float farYOffset; // offset the marker in the y direction
    public float closeYOffset;
    private float finalYOffset;

    public float minAlphaDistance; // distance where marker will have its maximum alpha value
    public float maxDrawDistance; // distance where marker will have its min alpha value
    public float defaultAlpha; // alpha value when interactable
    public float highAlpha; // high alpha value
    public float lowAlpha; // lowest alpha value

    private float deltaAlpha; // alpha value change per distance unit
    private float currentAlpha;
    private float distance;
    private float deltaDistance; // distance between minAlphaDistance and maxAlphaDistance
    private GameObject waypoint;

    [Header("Animation")]
    public float shakeDelay;
    public float shakeMax;
    public float shakeDuration;
    private float currentShake;

    private bool isAnimating;
    private bool doneAnimating;
    private float linearInterpolate = 0f;

    private float initialTimer;
    private float currentTimer;

    [Header("Persistant State Data")]
    public string[] dependentConditions; // conditions in persistant state data are required to trigger the Marker
    private bool dependentConditionsMet;
    public string[] onInteractConditions; // conditions in persistant state data are changed after initial interaction
    public string[] disableMarkerConditions;// conditions in persistant state data that disable the marker
    private bool disableMarker; // disable the marker if any disable marker conditions are true;

    private PersistantStateData persistantStateData;
    
	// Use this for initialization
	void Start () {
        waypoint = Instantiate(indicator);
        waypoint.GetComponent<RectTransform>().SetParent(mainCanvas.transform);
        waypoint.SetActive(false);

        // initialize alpha settings
        deltaDistance = maxDrawDistance - minAlphaDistance;
        deltaAlpha = (highAlpha - lowAlpha) / (deltaDistance);

        initialTimer = Time.time;
        isAnimating = false;
        doneAnimating = false;
            
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        dependentConditionsMet = false;
    }
	
    void IconShake()
    {
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
            doneAnimating = true;
            if (linearInterpolate > 0)
            {
                linearInterpolate -= (2 / shakeDuration) * Time.deltaTime;
            }
            currentShake = Mathf.Lerp(1, shakeMax, linearInterpolate);
        }

        if (linearInterpolate <= 0 && doneAnimating)
        {
            linearInterpolate = 0f;
            currentShake = Mathf.Lerp(1, shakeMax, linearInterpolate);
            isAnimating = false;
        }

    }
    void BeginTimer()
    {
        // Debug.Log("Cooldown: " + currentTimer + ", current Shake: " + currentShake);
        currentTimer = Time.time - initialTimer;

        if (currentTimer > shakeDelay)
        {
            doneAnimating = false;
            isAnimating = true;
            initialTimer = Time.time;
        }

        if (isAnimating)
        {
            IconShake();
        }

    }
    // Update is called once per frame
    void Update () {
        // check dependent conditions
        if (!dependentConditionsMet)
        {
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
                allConditionsTrue = true;
            }

            if (allConditionsTrue)
            {
                dependentConditionsMet = true;
            }
        }
        else
        {
            distance = Mathf.Abs((character.position - transform.position).magnitude);

            Vector3 rayCastDirection = (transform.position - mainCamera.transform.position).normalized;
            float rayCastCheck = Vector3.Dot(rayCastDirection, mainCamera.transform.forward);

            // check if marker is also drawing behind player, if so, don't draw
            if (rayCastCheck <= 0f || distance >= maxDrawDistance)
            {
                waypoint.SetActive(false);
            }
            else
            {
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

                if (!disableMarker)
                {
                    Vector3 offsetPosition = transform.position;

                    float distanceInterpolation = distance / maxDrawDistance;
                    finalYOffset = Mathf.Lerp(closeYOffset, farYOffset, distanceInterpolation);
                    offsetPosition.y = transform.position.y + finalYOffset;

                    // draw marker onto the canvas at same relative location as target
                    waypoint.GetComponent<RectTransform>().position = mainCamera.WorldToScreenPoint(offsetPosition);
                    waypoint.SetActive(true);
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
            BeginTimer();
            waypoint.transform.localScale = new Vector3(currentShake, currentShake, currentShake);

            float distancePercent = deltaDistance - (distance - minAlphaDistance);

            currentAlpha = distancePercent * deltaAlpha + lowAlpha;
            waypoint.GetComponent<CanvasGroup>().alpha = currentAlpha;
        }
        else
        {
            waypoint.transform.localScale = new Vector3(shakeMax, shakeMax, shakeMax);
            waypoint.GetComponent<CanvasGroup>().alpha = defaultAlpha;

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
