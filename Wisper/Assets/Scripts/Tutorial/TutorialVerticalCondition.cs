using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovementCondition : MonoBehaviour {

    public GameObject player;
    // variables relating to the player's distance
    public float distanceTravelRequired;
    private float playerDistanceTraveled;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;

    private bool lookTuturialMovementLockOver;
    private bool moveTutorialMovementUnlockOver;


    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        playerDistanceTraveled = 0;

        if((bool)persistantStateData.stateConditions["TutorialLookFinished"])
        {
            lookTuturialMovementLockOver = true;
            moveTutorialMovementUnlockOver = true;
        }
        else
        {
            lookTuturialMovementLockOver = false;
            moveTutorialMovementUnlockOver = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((bool)persistantStateData.stateConditions["StartupShrineDialogue"] && !(bool)persistantStateData.stateConditions["TutorialFirstDialogueFinished"] && !lookTuturialMovementLockOver)
        {
            lookTuturialMovementLockOver = true;
            Debug.Log("Disable movement for tutorial");
            player.GetComponent<PlayerMovement>().DisableMovement();
        }

        if ((bool)persistantStateData.stateConditions["TutorialLookFinished"] && !moveTutorialMovementUnlockOver)
        {
            moveTutorialMovementUnlockOver = true;
            player.GetComponent<PlayerMovement>().EnableMovement();
        }

        // player distance travelled
        playerDistanceTraveled += player.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

        // if player has traveled the required distance, update tutorial condition
        if (playerDistanceTraveled > distanceTravelRequired)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
