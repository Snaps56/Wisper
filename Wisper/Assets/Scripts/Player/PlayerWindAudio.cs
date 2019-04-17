using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerWindAudio : MonoBehaviour {

    public Rigidbody playerrigidbody;
    public AudioSource source;
    private float finalVelocity;

    public GameObject abilitiesCollider;
    public AudioClip throwAudioClip;

    private AudioSource throwingAudioSource;
    private bool isThrowing;

    private bool soundPlayed = false;


    // Use this for initialization
    void Start () {
        source.volume = 0;

    }

    void Awake()
    {
        // add the necessary AudioSources:
        throwingAudioSource = AddAudio(throwAudioClip, true, true, 1);

    }

    AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake,  float vol) {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
    }

// Update is called once per frame
void Update () {
        isThrowing = abilitiesCollider.GetComponent<ObjectThrow>().GetIsThrowingObjects();

        if (playerrigidbody.velocity.magnitude > 1)
        {
            if (1 - 1 / playerrigidbody.velocity.magnitude > 0.5)
            {
                source.volume = (1 - 1 / playerrigidbody.velocity.magnitude) * 0.5f;
                source.pitch = (1 - 1 / playerrigidbody.velocity.magnitude) * 2;
            }
        }
        else
        {
            source.volume = 0.25f;
            source.pitch = 1f;
        }
        throwingAudioSource.pitch = 1;
        if (!isThrowing)
        {
            soundPlayed = true;
        }
        else if (isThrowing && !throwingAudioSource.isPlaying)
        {
            if (soundPlayed)
            {
                soundPlayed = false;
                source.PlayOneShot(throwAudioClip);
                soundPlayed = false;
            }

        }


	}
}
