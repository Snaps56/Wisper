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
    public AudioClip liftAudioClip;

    private AudioSource throwingAudioSource;
    private bool isThrowing;

    private AudioSource liftAudioSource;
    private bool isLifting;

    private bool throwSoundPlayed = false;
    private bool liftSoundPlayed = false;

    // Use this for initialization
    void Start () {
        source.volume = 0;

    }

    void Awake()
    {
        // add the necessary AudioSources:
        throwingAudioSource = AddAudio(throwAudioClip, true, true, 1);
        liftAudioSource = AddAudio(liftAudioClip, true, true, 1);
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
        isLifting = abilitiesCollider.GetComponent<ObjectLift>().GetIsLiftingObjects();

        //Code to make passive audio
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

        //Code for ability noise
        //Throw noise
        if (!isThrowing)
        {
            throwSoundPlayed = true;
        }
        else if (isThrowing && !throwingAudioSource.isPlaying)
        {
            if (throwSoundPlayed)
            {
                throwingAudioSource.PlayOneShot(throwAudioClip);
                throwSoundPlayed = false;
            }
        }

        //Lift noise
        if (!isLifting)
        {
            liftSoundPlayed = true;
        }
        else if (isLifting && !liftAudioSource.isPlaying)
        {
            if (liftSoundPlayed)
            {
                liftAudioSource.PlayOneShot(liftAudioClip);
                liftSoundPlayed = false;
            }
        }

        ////Repeating 
        //if (isLifting && !liftAudioSource.isPlaying)
        //{
        //    liftAudioSource.PlayOneShot(liftAudioClip);
        //}


    }
}
