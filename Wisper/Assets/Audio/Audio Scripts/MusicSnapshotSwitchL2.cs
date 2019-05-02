using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicSnapshotSwitchL2 : MonoBehaviour {
    
    public static MusicSnapshotSwitchL2 L2AudioScript = null;
    private AudioSource myL2AudioSource;
    public AudioMixerSnapshot mySnapshot;
    public AudioMixerSnapshot mySnapshot2;
	public float fadeTime = 3.0f;
    public float delayTime = 0.0f;
    private bool needsToBeFaded = false;
    private float timeToStop = 0.0f;
    private Collider playerCollider;

    // Use this for initialization
    void Start () {
        myL2AudioSource = GetComponent<AudioSource>();
        playerCollider = PlayerPersistance.player.transform.Find("Abilities Collider").GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider col) {
        if(col == playerCollider)
        {
            myL2AudioSource.Play();
            mySnapshot.TransitionTo(fadeTime);
        }
	}

    void OnTriggerExit(Collider col)
    {
        if(col == playerCollider)
        {
            mySnapshot2.TransitionTo(fadeTime);
        }
        //Debug.Log("audio OnTriggerExit clip: " + myAudioSource.clip.name);
    }


}
