using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialVerticalCondition : MonoBehaviour {

    public GameObject player;
    // variables relating to the player's distance
    public float distanceVerticalRequired;
    private float playerDistanceTraveled;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;

    public string alternateCondition;
    private bool conditionCheck;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        playerDistanceTraveled = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // player distance travelled
        conditionCheck = (bool)persistantStateData.stateConditions[alternateCondition];

        playerDistanceTraveled += player.GetComponent<Rigidbody>().velocity.y * Time.deltaTime;
        // if player has traveled the required distance, update tutorial condition
        if (playerDistanceTraveled > distanceVerticalRequired || conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
