using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainCheck : MonoBehaviour {

    public float fadeDuration;
    
    private bool doneFading;

    private float initialTime;
    private float timeCounter;
    private bool initialTimeInitialized;

    private PersistantStateData persistantStateData;
    private bool updatedPSD;

    // Use this for initialization
    void Start () {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();

        initialTime = 0;
        initialTimeInitialized = false;
        doneFading = false;
        updatedPSD = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (!doneFading)
        {
            if (!initialTimeInitialized)
            {
                initialTime = Time.time;
                initialTimeInitialized = true;
            }
            timeCounter = Time.time - initialTime;

            if (timeCounter > fadeDuration)
            {
                UpdatePSD();
                doneFading = true;
            }
        }
	}
    void UpdatePSD()
    {
        if (!updatedPSD)
        {
            Debug.Log("Updated PSD");
            persistantStateData.stateConditions["StartupFadeFinished"] = true;
            persistantStateData.updateCount++;

            persistantStateData.stateConditions["StartupShrineDialogue"] = true;
            persistantStateData.updateCount++;

            updatedPSD = true;
        }
    }
}
