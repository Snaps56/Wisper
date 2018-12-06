using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractCondition : MonoBehaviour {

    // Game objects
    public string dependentCondition;
    public GameObject shrine;
    public Transform player;

    // variables relating to the Persistant State Data
    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;
    private bool conditionCheck;

    // variables reliant on distance between shrine and the player
    private float maxDistanceTrigger;
    private float distance;

    // Use this for initialization
    void Start ()
    {
        maxDistanceTrigger = shrine.GetComponent<UI_Marker>().maxDrawDistance;
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        tutorialCondition = GetComponent<TutorialCondition>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // check if player is within range to trigger the tutorial
        distance = (shrine.transform.position - player.position).magnitude;
        if (distance < maxDistanceTrigger)
        {
            persistantStateData.ChangeStateConditions("TutorialWithinShrineRange", true);
        }
        
        // check if tutorial is completed, then update the tutorials dependent conditions

        bool debugCheck = (bool)persistantStateData.stateConditions["ShrineFirstConversation"];
        Debug.Log("ShrineFirstConversation: " + debugCheck);

        conditionCheck = (bool)persistantStateData.stateConditions[dependentCondition];
        if (conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
