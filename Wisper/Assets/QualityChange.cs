using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityChange : MonoBehaviour {

    string[] names = QualitySettings.names;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetLow()
    {
        QualitySettings.SetQualityLevel(0);
    }
    public void SetMedium()
    {
        QualitySettings.SetQualityLevel(2);
    }
    public void SetHigh()
    {
        QualitySettings.SetQualityLevel(5);
    }
}
