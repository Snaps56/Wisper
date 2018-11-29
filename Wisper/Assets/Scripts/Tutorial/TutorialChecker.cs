using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour {
    
    public TutorialCondition tutorialCondition;

    public string[] initialConditions; // conditions in persistant state data required to run this tutorial
    private bool [] initialConditionBools;

    public string[] changeConditions; // conditions in persistant state data are changed after tutorial is finished

    private bool tutorialConditionMet;

    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start () {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        initialConditionBools = new bool[initialConditions.Length];
    }
	
	// Update is called once per frame
	void Update ()
    {
        for (int i = 0; i < initialConditions.Length; i++)
        {
            initialConditionBools[i] = (bool)persistantStateData.stateConditions[initialConditions[i]];
        }
        tutorialConditionMet = tutorialCondition.GetCondition();
    }
    public bool TutorialConditionMet()
    {
        return tutorialConditionMet;
    }
    public bool InitialConditionsMet()
    {
        for (int i = 0; i < initialConditionBools.Length; i++)
        {
            if (!initialConditionBools[i])
            {
                return false;
            }
        }
        return true;
    }
    public void updatePSD()
    {
        for (int i = 0; i < changeConditions.Length; i++)
        {
            persistantStateData.ChangeStateCondition(changeConditions[i], true);
            // Debug.Log(changeConditions[i] + ", " + (bool)persistantStateData.stateConditions[changeConditions[i]]);
        }
    }
}
