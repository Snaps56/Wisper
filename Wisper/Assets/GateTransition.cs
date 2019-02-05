using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateTransition : MonoBehaviour {

    public CanvasGroup blackFade;
    public Transform player;
    public GameObject transitionText;
    public string nextSceneName;
    public float minDistance;
    private bool startedAsync = false;
    private bool startedSceneLoad = false;

    private bool doFade = false;
    private bool doneFade = false;
    private float fadeDuration = 3f;
    private float fadeRate;

    private float currentDistance;
    private AsyncOperation async;

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
            if (Input.GetButtonDown("PC_Key_Interact") || Input.GetButtonDown("XBOX_Button_A"))
            {
                doFade = true;
            }
        }
        else
        {
            transitionText.SetActive(false);
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
            if (doneFade)
            {
                if (!startedAsync)
                {
                    startedAsync = true;
                    async = SceneManager.LoadSceneAsync(nextSceneName);
                    async.allowSceneActivation = false;
                }
                else
                {
                    // Debug.Log(async.progress);
                    if (async.progress >= 0.9f)
                    {
                        async.allowSceneActivation = true;
                    }
                }
            }
        }
    }
}
