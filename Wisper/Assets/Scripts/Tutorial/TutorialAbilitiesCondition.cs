using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAbilitiesCondition : MonoBehaviour {

    public string [] dependentConditions;

    private TutorialCondition tutorialCondition;
    private PersistantStateData persistantStateData;
    private bool currentConditionsMet;
    private bool conditionCheck;

    private bool conditionDone;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        //persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        persistantStateData = PersistantStateData.persistantStateData;
        conditionDone = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (conditionCheck && !conditionDone)
        {
            tutorialCondition.SetCondition(true);
            conditionDone = true;
        }
        else
        {
            currentConditionsMet = true;
            for (int i = 0; i < dependentConditions.Length; i++)
            {
                bool currentCondition = (bool)persistantStateData.stateConditions[dependentConditions[i]];
                if (!currentCondition)
                {
                    currentConditionsMet = false;
                }
            }
            if (currentConditionsMet)
            {
                conditionCheck = true;
            }
        }
    }
}
