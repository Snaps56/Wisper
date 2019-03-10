using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    private bool doneLoading;

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
        loadingText = GetComponentInChildren<Text>();
        // loadingText.text = "Loading.";
        //loadingText.text = "To Be Continued...";
        currentDot = initDot;

        StartCoroutine(AnimateLoadText());
    }

    // Update is called once per frame
    public void SetIsDoneLoading(bool setDone)
    {
        doneLoading = true;
    }
    IEnumerator AnimateLoadText()
    {
        float initialTime = Time.realtimeSinceStartup;
        float currentTime = initialTime;
        float dotInterval = 0.1f;
        int maxDots = 3;
        int currentDots = 0;
        loadingText.text = "Loading";
        
        while (!doneLoading)
        {
            currentTime = Time.realtimeSinceStartup - initialTime;
            Debug.Log(currentTime);

            if (currentTime > dotInterval)
            {
                initialTime = Time.realtimeSinceStartup;
                currentTime = initialTime;
                currentDots++;
                if (currentDots > maxDots)
                {
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
