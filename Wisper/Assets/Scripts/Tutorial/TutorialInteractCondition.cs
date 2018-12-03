using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractCondition : MonoBehaviour {

    public string dependentCondition;
    public GameObject shrine;
    public Transform player;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;
    private float maxDistanceTrigger;
    private bool conditionCheck;
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
        distance = (shrine.transform.position - player.position).magnitude;

        if (distance < maxDistanceTrigger)
        {
            persistantStateData.ChangeStateCondition("TutorialWithinShrineRange", true);
        }
        
        conditionCheck = (bool)persistantStateData.stateConditions[dependentCondition];
        
        if (conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
