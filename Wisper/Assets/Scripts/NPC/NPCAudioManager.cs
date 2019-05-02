using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAudioManager : MonoBehaviour
{
    //Old Shellster Audio
    public AudioClip[] oldShellsterAudioClips;

    //Young Shellster Audio
    public AudioClip[] youngShellsterAudioClips;

    //Shrine Audio
    public AudioClip[] shrineAudioClips;

    // Adult Shellster Audio
    public AudioClip[] adultShellsterAudioClips;

    //Audio Source
    private AudioSource audioSource;

    //0: Old Shellster, 1: Young Shellster, 2: Shrine
    public int NPCType = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void playOldShellsterClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(oldShellsterAudioClips[Random.Range(0, oldShellsterAudioClips.Length)]);
        }
    }
    private void playYoungShellsterClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(youngShellsterAudioClips[Random.Range(0, youngShellsterAudioClips.Length)]);
        }
    }
    private void playShrineClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(shrineAudioClips[Random.Range(0, shrineAudioClips.Length)]);
        }
    }
    private void playAdultShellsterClip()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(adultShellsterAudioClips[Random.Range(0, adultShellsterAudioClips.Length)]);
        }
    }

    public void Play()
    {
        //Old Shellster
        if (NPCType == 0)
        {
            playOldShellsterClip();
        }
        //Young Shellster
        else if (NPCType == 1)
        {
            playYoungShellsterClip();
        }
        //Shrine
        else if (NPCType == 2)
        {
            playShrineClip();
        }
        //Shrine
        else if (NPCType == 3)
        {
            playAdultShellsterClip();
        }
    }
}
