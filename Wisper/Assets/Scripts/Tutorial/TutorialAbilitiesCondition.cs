using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAbilitiesCondition : MonoBehaviour {

    public string dependentCondition;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;
    private bool conditionCheck;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // update that tutorial is met if the dependent condition is true
        conditionCheck = (bool)persistantStateData.stateConditions[dependentCondition];

        if (conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
