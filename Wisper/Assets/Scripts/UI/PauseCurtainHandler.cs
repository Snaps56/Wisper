using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCurtainHandler : MonoBehaviour {
    
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
	}
    public void OnFadeComplete()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(BeginLoadAsynchronous(1));
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