using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;

    void Awake()
    {
        // Makes sure that only one audiomanager is active at a time.
        if (instance == null)
        {
           instance = this;
           
        }
        else
        {
           Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Adds audiosource for each audio file with selected attributes
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }
    // Plays Main Theme
    void Start()
    {
        Play("windChimeSong");
    }

    // Finds and plays the song
    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

}
