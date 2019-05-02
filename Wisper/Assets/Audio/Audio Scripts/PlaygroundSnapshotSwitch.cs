using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlaygroundSnapshotSwitch : MonoBehaviour {
    
	public AudioMixerSnapshot onSnapshot; //Default
    public AudioMixerSnapshot offSnapshot; //Snapshot on
	public float fadeTime = 3.0f;
    public float delayTime = 0.0f;
    private ActivateCutscene activateCutsceneScript;

    bool lastCutsceneActiveState = false;


	// Use this for initialization
	void Start () {
        activateCutsceneScript = GameObject.Find("CutsceneManager").GetComponent<ActivateCutscene>();
        if(lastCutsceneActiveState != activateCutsceneScript.CutscenePlaying)
        {
            lastCutsceneActiveState = activateCutsceneScript.CutscenePlaying;
            if(lastCutsceneActiveState == true)
            {
                // DO THING WITH OFF SNAPSHOT HERE TO TUNE DOWN THE VOLUME
            }
            else
            {
                // DO THING WITH ON SNAPSHOT HERE TO TUNE UP THE VOLUME
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
                // DO THING WITH OFF SNAPSHOT HERE TO TUNE DOWN THE VOLUME
            }
            else
            {
                // DO THING WITH ON SNAPSHOT HERE TO TUNE UP THE VOLUME
            }
        }

    }
    
}
