using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlaygroundSnapshotSwitch : MonoBehaviour {
    
    public AudioMixerSnapshot softSnapshot; 
    public AudioMixerSnapshot normalSnapshot; 
	public float fadeIn = 0.0f;
    public float fadeOut = 3.0f;
    private ActivateCutscene activateCutsceneScript;

    bool lastCutsceneActiveState = false;


	// Use this for initialization
	void Awake () {
        activateCutsceneScript = GameObject.Find("CutsceneManager").GetComponent<ActivateCutscene>();
        if(lastCutsceneActiveState != activateCutsceneScript.CutscenePlaying)
        {
            lastCutsceneActiveState = activateCutsceneScript.CutscenePlaying;
            if(lastCutsceneActiveState == true)
            {
                softSnapshot.TransitionTo(fadeIn);
            }
            else
            {
                normalSnapshot.TransitionTo(fadeOut);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (lastCutsceneActiveState != activateCutsceneScript.CutscenePlaying)
        {
            lastCutsceneActiveState = activateCutsceneScript.CutscenePlaying;
            if (lastCutsceneActiveState == true)
            {
                softSnapshot.TransitionTo(fadeIn);
            }
            else
            {
                normalSnapshot.TransitionTo(fadeOut);
            }
        }

    }
    
}
