using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsBasicsCondition : MonoBehaviour {

    public float shrineProximityTrigger;

    private GameObject shrine;
    public GameObject player;
    private GameObject mainCamera;

    private float currentProximity;

    private float playerDistanceTraveled;
    private TutorialCondition tutorialCondition;

    private bool doneTutorial = false;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        shrine = GameObject.Find("Shrine");
        player = GameObject.Find("Player");
        mainCamera = player.transform.Find("Main Camera").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        // if condition met, update the psd variable
        
        if (!(bool)PersistantStateData.persistantStateData.stateConditions["TutorialFirstDialogueFinished"])
        {
            player.GetComponent<PlayerMovement>().DisableMovement();
            mainCamera.GetComponent<CameraOptimized>().DisableCameraMovement();
        }
        else
        {
            player.GetComponent<PlayerMovement>().EnableMovement();
            mainCamera.GetComponent<CameraOptimized>().EnableCameraMovement();
        }

        currentProximity = Vector3.Distance(shrine.transform.position, player.transform.position);

        if (currentProximity < shrineProximityTrigger)
        {
            tutorialCondition.SetCondition(true);
            doneTutorial = true;
        }

    }
}
