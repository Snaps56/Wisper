using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInteractCondition : MonoBehaviour {

    public string dependentCondition;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;
    private bool conditionCheck;

    // Use this for initialization
    void Start ()
    {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        tutorialCondition = GetComponent<TutorialCondition>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        conditionCheck = (bool)persistantStateData.stateConditions[dependentCondition];
        
        if (conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
