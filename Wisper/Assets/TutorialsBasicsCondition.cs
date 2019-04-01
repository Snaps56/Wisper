using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialsBasicsCondition : MonoBehaviour {

    public float shrineProximityTrigger;

    private Transform shrineTransform;
    private GameObject player;
    private GameObject mainCamera;

    private float currentProximity;

    private float playerDistanceTraveled;
    private TutorialCondition tutorialCondition;

    private bool doneTutorial = false;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        shrineTransform = GameObject.Find("Shrine").transform;
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
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

        currentProximity = Vector3.Distance(shrineTransform.position, player.transform.position);

        if (currentProximity < shrineProximityTrigger)
        {
            tutorialCondition.SetCondition(true);
            doneTutorial = true;
        }

    }
}
