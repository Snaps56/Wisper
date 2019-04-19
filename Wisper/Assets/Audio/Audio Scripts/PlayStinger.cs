using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class PlayStinger : MonoBehaviour
{
    //public AudioClip soundEffect;
    //AudioSource myAudioSource;
    // Use this for initialization
    void Start()
    {
        //myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //AudioSource myAudioSource = GetComponent<AudioSource>();
        //if (Input.GetKeyDown(KeyCode.LeftShift))
            //myAudioSource.PlayOneShot(soundEffect, 1.0f);
    }

    void OnTriggerEnter()
    {
        AudioSource myAudioSource = GetComponent<AudioSource>();
        myAudioSource.Play ();
    }
}