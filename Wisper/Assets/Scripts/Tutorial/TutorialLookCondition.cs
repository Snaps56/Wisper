using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLookCondition : MonoBehaviour
{
    public GameObject shrine;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;
    //The camera's forward vector
    private Vector3 cameraForward;
    //The vector between the shrine and camera
    private Vector3 toShrine;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start()
    {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        tutorialCondition = GetComponent<TutorialCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((bool)persistantStateData.stateConditions["StartupShrineDialogue"] && !(bool)persistantStateData.stateConditions["TutorialFirstDialogueFinished"])
        {
            mainCamera.GetComponent<CameraOptimized>().DisableCameraMovement();
        }

        if ((bool)persistantStateData.stateConditions["TutorialFirstDialogueFinished"])
        {
            mainCamera.GetComponent<CameraOptimized>().EnableCameraMovement();
        }

        //Update the forward vector
        cameraForward = mainCamera.transform.forward;

        //Find direction between the shrine and camera
        toShrine = shrine.transform.position - mainCamera.transform.position;


        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toShrine.normalized) > dotProductAngle)
        {
            // Debug.Log(Vector3.Dot(cameraForward.normalized, toShrine.normalized));
            tutorialCondition.SetCondition(true);
        }
    }
}
