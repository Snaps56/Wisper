using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class FlareVignette : MonoBehaviour {
    
    private Transform shrine;
    public PostProcessingProfile postProcessingProfile;
    private VignetteModel.Settings vignetteSettings;

    public float defaultVignette = 0.2f;
    private float finalVignette;

    // Use this for initialization
    void Start () {
        shrine = GameObject.Find("Shrine").transform;
        vignetteSettings = postProcessingProfile.vignette.settings;
	}
	
	// Update is called once per frame
	void Update () {
        float flareDot = Vector3.Dot((shrine.transform.position - transform.position).normalized, transform.forward.normalized);

        finalVignette = defaultVignette + (1 - flareDot) * (.5f);

        if (finalVignette > 0.5f)
        {
            finalVignette = 0.5f;
        }
        vignetteSettings.intensity = finalVignette;

        Debug.Log("Flare Dot Product: " + flareDot + ", vignette intensity: " + finalVignette);


        postProcessingProfile.vignette.settings = vignetteSettings;
	}
}
