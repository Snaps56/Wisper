using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    public GameObject loadingScreen;
    public GameObject logo;
    public GameObject text;

    public CanvasGroup blackFade;
    public Transform player;
    public GameObject transitionText;
    public string nextSceneName;
    public float minDistance;
    private bool startedAsync = false;

    private bool doFade = false;
    private bool doneFade = false;
    private float fadeDuration = 2f;
    private float fadeRate;

    private float currentDistance;
    private AsyncOperation async;

    private float delayInitial;
    private float delayCurrent;
    private float delayDuration = 5.0f;

    // Use this for initialization
    void Start () {
        blackFade.alpha = 0;
        fadeRate = Time.fixedDeltaTime / fadeDuration;
	}

    // Update is called once per frame
    void Update() {
        currentDistance = Vector3.Distance(player.position, transform.position);
        //Debug.Log(currentDistance);

        if (currentDistance < minDistance)
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
        }
        if (startedAsync)
        {
            if (Time.time > delayInitial + delayDuration)
            {
                async.allowSceneActivation = true;
            }
        }
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
                loadingScreen.SetActive(true);
                delayInitial = Time.time;
                startedAsync = true;
                StartCoroutine(LoadAsynchronously(nextSceneName));
            }
        }
    }
    IEnumerator LoadAsynchronously(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            //Debug.Log(async.progress);
            yield return null;
        }
    }
}
