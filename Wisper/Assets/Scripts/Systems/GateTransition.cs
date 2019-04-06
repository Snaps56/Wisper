using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    // variables that handle game objects
    private GameObject loadingScreen;
    private CanvasGroup blackFade;
    private Transform player;
    public GameObject transitionText;

    // variables that handle distance of player to the gate
    public float minDistance;
    private float currentDistance;
    private bool withinGateRange = false;

    [Header("Scene Loading")]

    // variables that handle and manage scenes
    public bool useSceneNumberInstead;
    public string nextSceneName;
    public int nextSceneNumber;
    private LoadingScreen loadingScreenScript;

    // variables that handle fading and async
    private bool startedAsync = false;
    private bool doFade = false;
    private bool doneFade = false;
    private float fadeDuration = 2f;
    private float fadeRate;

    private AsyncOperation async;
    private float delayInitial;
    private float delayCurrent;
    private float delayDuration = 10.0f;


    // Use this for initialization
    void Start () {
        // initialize game objects
        loadingScreen = GameObject.Find("Loading Screen");
        player = GameObject.Find("Player").transform;
        transitionText.SetActive(false);
        blackFade = GameObject.Find("Faded").GetComponent<CanvasGroup>();
        blackFade.alpha = 0;

        fadeRate = Time.fixedDeltaTime / fadeDuration;

        // find loading screen only if unable to find via public variable
        if (loadingScreen == null)
        {
            loadingScreen = GameObject.Find("MainCanvas").transform.Find("Loading Screen").gameObject;
            loadingScreenScript = loadingScreen.GetComponent<LoadingScreen>();
        }
    }

    // Update is called once per frame
    void Update() {

        // check if the player is within distance of the gate
        currentDistance = Vector3.Distance(player.position, transform.position);

        if (currentDistance < minDistance)
        {
            withinGateRange = true;
        }
        else
        {
            withinGateRange = false;
        }

        // if the player is within the gate range and presses a button, begin fading
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
    }
    public bool GetWithinGateRange()
    {
        return withinGateRange;
    }
    // used fixed update for a constant black fade for all devices
    private void FixedUpdate()
    {
        FadeChecker();
    }

    // fade the screen to black if the black fade is triggered
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

            // if the black fade is done, begin async load of the next scene
            if (doneFade && !startedAsync)
            {
                if (loadingScreen == null)
                {
                    loadingScreen = GameObject.Find("MainCanvas").transform.Find("Loading Screen").gameObject;
                    loadingScreenScript = loadingScreen.GetComponent<LoadingScreen>();
                }
                loadingScreen.SetActive(true);
                delayInitial = Time.time;

                // call a coroutine that runs independently and asynshronously to load the next scene
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

    // coroutine that runs async to load scene based on given scene name
    IEnumerator BeginLoadAsynchronous(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        
        async.allowSceneActivation = true;
        startedAsync = true;

        if (async.isDone)
        {
            loadingScreenScript.SetIsDoneLoading(true);
        }
        while (!async.isDone)
        {
            yield return null;
        }
    }

    // coroutine that runs async to load scene based on given scene number
    IEnumerator BeginLoadAsynchronous(int sceneNumber)
    {
        async = SceneManager.LoadSceneAsync(sceneNumber);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        
        async.allowSceneActivation = true;
        startedAsync = true;

        if (async.isDone)
        {
            loadingScreenScript.SetIsDoneLoading(true);
        }
        while (!async.isDone)
        {
            yield return null;
        }
    }
}
