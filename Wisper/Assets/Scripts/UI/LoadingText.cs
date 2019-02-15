using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour {

    private bool doneLoading;

    private Text loadingText;
    private float dotDelay = 0.75f;
    private int currentDot;
    private int initDot = 0;
    private int maxDot = 3;
    private float initDelay;
    private bool hasInitiated;

	// Use this for initialization
	void Start () {
        loadingText = GetComponent<Text>();
        // loadingText.text = "Loading.";
        loadingText.text = "To Be Continued...";
        currentDot = initDot;
	}
	
	// Update is called once per frame
	void Update () {
        /*
		if (transform.parent.gameObject.activeInHierarchy && !hasInitiated)
        {
            hasInitiated = true;
            initDelay = Time.time;
        }
        if (hasInitiated)
        {
            if (Time.time > initDelay + dotDelay)
            {
                if (currentDot < maxDot)
                {
                    currentDot++;
                }
                else
                {
                    currentDot = initDot;
                }
                initDelay = Time.time;
            }
        }
        loadingText.text = "Loading";
        for (int i = 0; i < currentDot; i++)
        {
            loadingText.text += ".";
        }
        */
	}
}
