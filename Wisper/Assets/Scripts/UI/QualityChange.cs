using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class QualityChange : MonoBehaviour {

    public PostProcessingProfile [] postProcessingProfiles;
    string[] names;

    // Use this for initialization
    void Start () {
		names = QualitySettings.names;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetLow()
    {
        for (int i = 0; i < postProcessingProfiles.Length; i++)
        {
            postProcessingProfiles[i].depthOfField.enabled = false;
            postProcessingProfiles[i].ambientOcclusion.enabled = false;
            postProcessingProfiles[i].bloom.enabled = false;
        }
        QualitySettings.SetQualityLevel(0);
    }
    public void SetMedium()
    {
        for (int i = 0; i < postProcessingProfiles.Length; i++)
        {
            postProcessingProfiles[i].depthOfField.enabled = true;
            postProcessingProfiles[i].ambientOcclusion.enabled = false;
            postProcessingProfiles[i].bloom.enabled = false;
        }
        QualitySettings.SetQualityLevel(2);
    }
    public void SetHigh()
    {
        for (int i = 0; i < postProcessingProfiles.Length; i++)
        {
            postProcessingProfiles[i].depthOfField.enabled = true;
            postProcessingProfiles[i].ambientOcclusion.enabled = true;
            postProcessingProfiles[i].bloom.enabled = true;
        }
        QualitySettings.SetQualityLevel(4);
    }
}
