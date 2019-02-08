using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour {
    
    // variables relating to the tutorial conditions
    public TutorialCondition tutorialCondition;
    private bool tutorialConditionMet;

    public string[] initialConditions; // conditions in persistant state data required to run this tutorial
    private bool [] initialConditionBools;

    public string[] changeConditions; // conditions in persistant state data are changed after tutorial is finished

    private bool sceneChecker;
    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start () {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        initialConditionBools = new bool[initialConditions.Length];

        bool initChangeCondition = true;
        for (int i = 0; i < changeConditions.Length; i++)
        {
            sceneChecker = (bool)persistantStateData.stateConditions[changeConditions[i]];
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        // check every frame if initial conditions have been met
        for (int i = 0; i < initialConditions.Length; i++)
        {
            initialConditionBools[i] = (bool)persistantStateData.stateConditions[initialConditions[i]];
        }
        tutorialConditionMet = tutorialCondition.GetCondition();
    }

    // return whether if tutorial is completed to trigger another event
    public bool TutorialConditionMet()
    {
        return tutorialConditionMet;
    }

    // Check if the initial conditions if this tutorial have been been completed before tutorial can start
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

    // update the Persistant State Data's variables
    public void updatePSD()
    {
        for (int i = 0; i < changeConditions.Length; i++)
        {
            persistantStateData.ChangeStateConditions(changeConditions[i], true);
        }
    }
}
