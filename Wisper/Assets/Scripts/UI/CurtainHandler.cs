using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainHandler : MonoBehaviour {

    public bool usePsdToTriggerFade;
    public string psdToTriggerFadeOut;
    public SceneLoadMethod sceneLoadMethod;
    public int fadeOutSceneLoadNumber = 1;
    public string fadeOutSceneName;

    
    private Animator curtainAnimator;

    private AsyncOperation async;
    private LoadingScreen loadingScreenScript;
    private GameObject loadingScreen;

    // Use this for initialization
    void Start () {
        curtainAnimator = GetComponent<Animator>();
        loadingScreen = GameObject.Find("MainCanvas").transform.Find("Loading Screen").gameObject;
        loadingScreenScript = loadingScreen.GetComponent<LoadingScreen>();
    }
	
	// Update is called once per frame
	void Update () {
        // Fade Out after tutorial is done
        if (usePsdToTriggerFade)
        {
            if ((bool)PersistantStateData.persistantStateData.stateConditions[psdToTriggerFadeOut])
            {
                curtainAnimator.SetTrigger("FadeOut");
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            curtainAnimator.SetTrigger("FadeOut");
        }
	}
    public void OnFadeComplete()
    {
        loadingScreen.SetActive(true);
        if (sceneLoadMethod.Equals(SceneLoadMethod.useSceneName))
        {
            StartCoroutine(BeginLoadAsynchronous(fadeOutSceneName));
            //SceneManager.LoadScene(fadeOutSceneName);
        }
        else
        {
            StartCoroutine(BeginLoadAsynchronous(fadeOutSceneLoadNumber));
            //SceneManager.LoadScene(fadeOutSceneLoadNumber);
        }
    }
    // coroutine that runs async to load scene based on given scene name
    IEnumerator BeginLoadAsynchronous(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        Application.backgroundLoadingPriority = ThreadPriority.BelowNormal;

        async.allowSceneActivation = true;
        
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
        
        while (!async.isDone)
        {
            yield return null;
        }
    }
}

public enum SceneLoadMethod { useSceneNumber, useSceneName}