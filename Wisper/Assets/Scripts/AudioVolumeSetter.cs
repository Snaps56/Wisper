using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSetter : MonoBehaviour {

    public AudioMixer audioMixer;
    private Slider audioSlider;
    private float audioVolume;

    // Use this for initialization
    void Start () {
        audioSlider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        audioVolume = audioSlider.value;
    }
    public void SetSoundVolume()
    {
        audioMixer.SetFloat("soundVolume", audioVolume);
    }
    public void SetMusicVolume()
    {
        audioMixer.SetFloat("musicVolume", audioVolume);
    }
}
