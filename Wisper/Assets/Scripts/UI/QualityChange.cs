using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class QualityChange : MonoBehaviour {

    public PostProcessingProfile ppProfile;
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
        ppProfile.depthOfField.enabled = false;
        ppProfile.ambientOcclusion.enabled = false;
        ppProfile.bloom.enabled = false; ;
        QualitySettings.SetQualityLevel(0);
    }
    public void SetMedium()
    {
        ppProfile.depthOfField.enabled = true;
        ppProfile.ambientOcclusion.enabled = false;
        ppProfile.bloom.enabled = false;
        QualitySettings.SetQualityLevel(2);
    }
    public void SetHigh()
    {
        ppProfile.depthOfField.enabled = true;
        ppProfile.ambientOcclusion.enabled = true;
        ppProfile.bloom.enabled = true;
        QualitySettings.SetQualityLevel(4);
    }
}
