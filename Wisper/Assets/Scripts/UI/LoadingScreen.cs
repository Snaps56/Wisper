using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    private bool doneLoading;

    private Transform loadingIcon;
    private Text loadingText;
    private float dotDelay = 0.75f;
    private int currentDot;
    private int initDot = 0;
    private int maxDot = 3;
    private float initDelay;
    private bool hasInitiated;

    // Use this for initialization
    void Start()
    {
        loadingIcon = GameObject.Find("SunshoreLoading").transform;
        loadingText = GetComponentInChildren<Text>();
        // loadingText.text = "Loading.";
        //loadingText.text = "To Be Continued...";
        currentDot = initDot;
        StartCoroutine(AnimateLoadText());
    }

    // Update is called once per frame
    public void SetIsDoneLoading(bool setDone)
    {
        doneLoading = setDone;
    }
    IEnumerator AnimateLoadText()
    {
        float initialTime = Time.realtimeSinceStartup;
        float currentTime = initialTime;
        float dotInterval = 0.3f;
        int maxDots = 3;
        int currentDots = 0;
        if (loadingIcon == null)
        {
            loadingIcon = GameObject.Find("SunshoreLoading").transform;
        }
        if (loadingText == null)
        {
            loadingText = GetComponentInChildren<Text>();
        }
        loadingText.text = "Loading";
        
        while (!doneLoading)
        {
            Debug.Log("Do Loading CoRoutine");
            loadingIcon.Rotate(0, 1f, 0);
            //Debug.Log("Current Time: " + currentTime + ", Initial Time: " + initialTime);
            currentTime = Time.realtimeSinceStartup - initialTime;
            //Debug.Log(currentTime);

            if (currentTime > dotInterval)
            {
                //Debug.Log("Current Dot: " + currentDots + ", Max Dots: " + maxDots);
                initialTime = Time.realtimeSinceStartup;
                currentTime = 0;
                currentDots++;
                if (currentDots > maxDots)
                {
                    currentDots = 0;
                    loadingText.text = "Loading";
                }
                else
                {
                    loadingText.text += ".";
                }
            }
            yield return null;
        }
    }
}
