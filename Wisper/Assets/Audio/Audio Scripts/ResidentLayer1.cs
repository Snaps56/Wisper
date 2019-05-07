using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ResidentLayer1 : MonoBehaviour {
    
	public AudioMixerSnapshot mySnapshot; //Default
    public AudioMixerSnapshot mySnapshot2; //Snapshot on
	public float fadeTime = 3.0f;
    public float delayTime = 0.0f;
    private AudioSource myAudioSource;
    public AudioSource L2AudioSource;
    public AudioSource saxAudioSource;
    public AudioSource tamboAudioSource;
    public AudioSource panAudioSource;
    private Collider playerCollider;
    private bool needsToBeFaded = false;
    private float timeToStop = 0.0f;

	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();
        playerCollider = PlayerPersistance.player.transform.Find("Abilities Collider").GetComponent<Collider>();
        //panAudioSource = GameObject.Find("Shane").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown("q")){
            needsToBeFaded = false;
            myAudioSource.Play();
            L2AudioSource.Play();
            saxAudioSource.Play();
            tamboAudioSource.Play();
            panAudioSource.Play();

        }

        if(myAudioSource.isPlaying && needsToBeFaded){
            if(Time.time > timeToStop){
                myAudioSource.Stop();
                L2AudioSource.Stop();
                saxAudioSource.Stop();
                tamboAudioSource.Stop();
                panAudioSource.Stop();
                needsToBeFaded = false;
                //Debug.Log("audio Stop clip: " + myAudioSource.clip.name);

            }

        }
	
	}

    void OnTriggerEnter (Collider col) {
        if (col == playerCollider){
            needsToBeFaded = false;
            myAudioSource.Play();
            //L2AudioSource.Play();
            saxAudioSource.Play();
            tamboAudioSource.Play();
            panAudioSource.Play();
            //MusicSnapshotSwitchL2.L2AudioScript.myL2AudioSource 
            mySnapshot.TransitionTo(fadeTime);
            Debug.Log("audio OnTriggerEnter clip: " + myAudioSource.clip.name);

        }
        Debug.Log("audio OnTriggerEnter clip: " + myAudioSource.clip.name + "Outside Function");
	}

    void OnTriggerExit(Collider col){
        if (col == playerCollider){
            mySnapshot2.TransitionTo(fadeTime);
            timeToStop = Time.time + fadeTime + 0.1f;
            needsToBeFaded = true;
            Debug.Log("audio OnTriggerExit clip: " + myAudioSource.clip.name);

        }
        Debug.Log("audio OnTriggerEnter clip: " + myAudioSource.clip.name + "Outside Function");

    }
}
