using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainHandler : MonoBehaviour {

    public int fadeOutSceneLoadNumber = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(fadeOutSceneLoadNumber);
    }
}
