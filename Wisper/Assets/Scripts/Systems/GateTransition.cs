﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    private GameObject loadingScreen;
    private CanvasGroup blackFade;
    private Transform player;
    public GameObject transitionText;
    public float minDistance;

    [Header("Scene Loading")]

    public bool useSceneNumberInstead;
    public string nextSceneName;
    public int nextSceneNumber;
    private LoadingScreen loadingScreenScript;

    private bool startedAsync = false;

    private bool doFade = false;
    private bool doneFade = false;
    private float fadeDuration = 2f;
    private float fadeRate;

    private float currentDistance;
    private AsyncOperation async;

    private float delayInitial;
    private float delayCurrent;
    private float delayDuration = 10.0f;

    private bool withinGateRange = false;

    // Use this for initialization
    void Start () {
        loadingScreen = GameObject.Find("Loading Screen");
        player = GameObject.Find("Player").transform;
        transitionText.SetActive(false);
        blackFade = GameObject.Find("Faded").GetComponent<CanvasGroup>();
        blackFade.alpha = 0;

        fadeRate = Time.fixedDeltaTime / fadeDuration;

        if (loadingScreen == null)
        {
            loadingScreen = GameObject.Find("MainCanvas").transform.Find("Loading Screen").gameObject;
            loadingScreenScript = loadingScreen.GetComponent<LoadingScreen>();
        }
    }

    // Update is called once per frame
    void Update() {
        currentDistance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(currentDistance);

        if (currentDistance < minDistance)
        {
            withinGateRange = true;
        }
        else
        {
            withinGateRange = false;
        }
        if (withinGateRange)
        {
            transitionText.SetActive(true);
            if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_A"))
            {
                doFade = true;
            }
        }
        else
        {
            transitionText.SetActive(false);
            PersistantStateData.persistantStateData.ChangeStateConditions("TutorialGateTravelled", true);
        }
        
        if (startedAsync)
        {
        }
    }
    public bool GetWithinGateRange()
    {
        return withinGateRange;
    }
    private void FixedUpdate()
    {
        FadeChecker();
    }
    void FadeChecker()
    {
        if (doFade)
        {
            if (blackFade.alpha < 1 && !doneFade)
            {
                blackFade.alpha += fadeRate;
            }
            else
            {
                doneFade = true;
            }
            if (doneFade && !startedAsync)
            {
                if (loadingScreen == null)
                {
                    loadingScreen = GameObject.Find("MainCanvas").transform.Find("Loading Screen").gameObject;
                    loadingScreenScript = loadingScreen.GetComponent<LoadingScreen>();
                }
                loadingScreen.SetActive(true);
                delayInitial = Time.time;
                if (useSceneNumberInstead)
                {
                    StartCoroutine(BeginLoadAsynchronous(nextSceneNumber));
                }
                else
                {
                    StartCoroutine(BeginLoadAsynchronous(nextSceneName));
                }
            }
        }
    }
    IEnumerator BeginLoadAsynchronous(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        //async.allowSceneActivation = false;
        async.allowSceneActivation = true;
        startedAsync = true;
        if (async.isDone)
        {
            loadingScreenScript.SetIsDoneLoading(true);
            Debug.Log("Done loading! printed from coroutine after finished");
        }
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            yield return null;
        }
    }
    IEnumerator BeginLoadAsynchronous(int sceneNumber)
    {
        async = SceneManager.LoadSceneAsync(sceneNumber);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        //async.allowSceneActivation = false;
        async.allowSceneActivation = true;
        startedAsync = true;
        if (async.isDone)
        {
            loadingScreenScript.SetIsDoneLoading(true);
            Debug.Log("Done loading! printed from coroutine after finished");
        }
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            yield return null;
        }
    }
}
