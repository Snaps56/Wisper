using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuMusic : MonoBehaviour {
    
	public AudioMixerSnapshot mySnapshot; //Default snapshot on
    public AudioMixerSnapshot mySnapshot2;              
	public float fadeTime = .5f;
    public float fadeOut = 3.0f;
    public float delayTime = 0.0f;
    private AudioSource myAudioSource;
    //public AudioSource L2AudioSource;
    private bool needsToBeFaded = false;
    private float timeToStop = 0.0f;

    public Button startGame;


	// Use this for initialization
	void Start () {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Play();
        mySnapshot.TransitionTo(fadeTime);
        startGame.onClick.AddListener(FadeOut);
	}
	
	// Update is called once per frame
	void Update () {
        if(myAudioSource.isPlaying && needsToBeFaded){
            if(Time.time > timeToStop){
                myAudioSource.Stop();
                //L2AudioSource.Stop();
                needsToBeFaded = false;
                //Debug.Log("audio Stop clip: " + myAudioSource.clip.name);

            }

        }
	
	}

	//void OnTriggerEnter () {
 //       myAudioSource.Play();
 //       //L2AudioSource.Play();
 //       //MusicSnapshotSwitchL2.L2AudioScript.myL2AudioSource 
	//	mySnapshot.TransitionTo(fadeTime);
	//}

    void FadeOut()
    {
        mySnapshot2.TransitionTo(fadeOut);
        timeToStop = Time.time + fadeTime + 1.0f;
        needsToBeFaded = true;
        //Debug.Log("audio OnTriggerExit clip: " + myAudioSource.clip.name);
    }
}
