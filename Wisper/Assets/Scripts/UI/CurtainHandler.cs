using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainHandler : MonoBehaviour {

    public string psdToTriggerFadeOut;
    public SceneLoadMethod sceneLoadMethod;
    public int fadeOutSceneLoadNumber = 1;
    public string fadeOutSceneName;

    
    private Animator curtainAnimator;

	// Use this for initialization
	void Start () {
        curtainAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        // Fade Out after tutorial is done
        if ((bool)PersistantStateData.persistantStateData.stateConditions[psdToTriggerFadeOut])
        {
            curtainAnimator.SetTrigger("FadeOut");
        }
	}
    public void OnFadeComplete()
    {
        if (sceneLoadMethod.Equals(SceneLoadMethod.useSceneName))
        {
            SceneManager.LoadScene(fadeOutSceneName);
        }
        else
        {
            SceneManager.LoadScene(fadeOutSceneLoadNumber);
        }
    }
}

public enum SceneLoadMethod { useSceneNumber, useSceneName}