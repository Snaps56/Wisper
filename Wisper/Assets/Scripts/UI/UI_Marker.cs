using System.Collections;
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
    public float yOffset; // offset the marker in the y direction
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

	// Use this for initialization
	void Start () {
        waypoint = Instantiate(indicator);
        waypoint.GetComponent<RectTransform>().SetParent(mainCanvas.transform);
        waypoint.SetActive(false);

        // initialize alpha settings
        deltaDistance = maxDrawDistance - minAlphaDistance;
        deltaAlpha = (highAlpha - lowAlpha) / (deltaDistance);
    }
	
	// Update is called once per frame
	void Update () {
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
            Vector3 offsetPosition = transform.position;
            offsetPosition.y = transform.position.y + yOffset;

            // draw marker onto the canvas at same relative location as target
            waypoint.GetComponent<RectTransform>().position = mainCamera.WorldToScreenPoint(offsetPosition);
            waypoint.SetActive(true);

            // alpha scaling with distance
            if (distance > minAlphaDistance)
            {
                float distancePercent = deltaDistance - (distance - minAlphaDistance);

                currentAlpha = distancePercent * deltaAlpha + lowAlpha;
                waypoint.GetComponent<CanvasGroup>().alpha = currentAlpha;
            }
            else
            {
                waypoint.GetComponent<CanvasGroup>().alpha = defaultAlpha;
            }
        }
    }
}
