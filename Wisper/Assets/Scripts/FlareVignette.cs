using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class FlareVignette : MonoBehaviour
{

    private Transform shrine;
    public PostProcessingProfile postProcessingProfile;
    private VignetteModel.Settings vignetteSettings;

    public float defaultVignette = 0.2f;
    public float finalVignette = 0.5f;

    // Use this for initialization
    void Start()
    {
        shrine = GameObject.Find("Shrine").transform;
        vignetteSettings = postProcessingProfile.vignette.settings;
    }

    // Update is called once per frame
    void Update()
    {
        postProcessingProfile.vignette.settings = vignetteSettings;
        vignetteSettings.intensity = finalVignette;

        if (PersistantStateData.persistantStateData.enableDebugMode)
        {
            DoFinalVignette();
        }
        else
        {
            float flareDot = Vector3.Dot((shrine.transform.position - transform.position).normalized, transform.forward.normalized);

            if (!(bool)PersistantStateData.persistantStateData.stateConditions["TutorialBasicsFinished"])
            {
                finalVignette = defaultVignette + (1 - flareDot) * (.5f);

                if (finalVignette > 0.5f)
                {
                    finalVignette = 0.5f;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if ((bool)PersistantStateData.persistantStateData.stateConditions["TutorialBasicsFinished"])
        {
            DoFinalVignette();
        }
    }
    private void DoFinalVignette()
    {
        if (finalVignette > defaultVignette)
        {
            finalVignette -= 0.025f;
        }
    }
}
