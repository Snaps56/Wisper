using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicSnapshotSwitchL1 : MonoBehaviour {
    
	public AudioMixerSnapshot mySnapshot; //Default
    public AudioMixerSnapshot mySnapshot2; //Snapshot on
	public float fadeTime = 3.0f;
    public float delayTime = 0.0f;
    private AudioSource myAudioSource;
    public AudioSource L2AudioSource;
    public Collider audioCollider;
    private bool needsToBeFaded = false;
    private float timeToStop = 0.0f;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.playOnAwake = false;
        //audioCollider = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if(myAudioSource.isPlaying && needsToBeFaded){
            if(Time.time > timeToStop){
                myAudioSource.Stop();
                needsToBeFaded = false;
                //Debug.Log("audio Stop clip: " + myAudioSource.clip.name);

            }

        }
	
	}

    void OnTriggerEnter (Collider playerCollider) {
        if (playerCollider.CompareTag("Player")){
            myAudioSource.Play();
            L2AudioSource.Play();
            //MusicSnapshotSwitchL2.L2AudioScript.myL2AudioSource 
            mySnapshot.TransitionTo(fadeTime);
            
        }
	}

    void OnTriggerExit(Collider playerCollider)
    {
        if (playerCollider.CompareTag("Player"))
        {
            mySnapshot2.TransitionTo(fadeTime);
            timeToStop = Time.time + fadeTime + 2.0f;
            needsToBeFaded = true;
            L2AudioSource.Stop();
            //Debug.Log("audio OnTriggerExit clip: " + myAudioSource.clip.name);

        }
      
    }
}
