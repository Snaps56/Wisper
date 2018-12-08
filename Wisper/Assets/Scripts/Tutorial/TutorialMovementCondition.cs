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


    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        playerDistanceTraveled = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if ((bool)persistantStateData.stateConditions["StartupShrineDialogue"] && !(bool)persistantStateData.stateConditions["TutorialFirstDialogueFinished"])
        {
            player.GetComponent<PlayerMovement>().DisableMovement();
        }

        if ((bool)persistantStateData.stateConditions["TutorialLookFinished"])
        {
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
