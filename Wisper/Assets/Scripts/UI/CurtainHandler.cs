using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainHandler : MonoBehaviour {

    public int fadeOutSceneLoadNumber = 1;
    private Animator curtainAnimator;

	// Use this for initialization
	void Start () {
        curtainAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        // Fade Out after tutorial is done
        /*
        if ((bool)PersistantStateData.persistantStateData.stateConditions["DemoEnd"] || Input.GetKey(KeyCode.Y))
        {
            curtainAnimator.SetTrigger("FadeOut");
        }
        */
	}
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(fadeOutSceneLoadNumber);
    }
}
