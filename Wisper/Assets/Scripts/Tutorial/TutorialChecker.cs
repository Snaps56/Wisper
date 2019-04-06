using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecker : MonoBehaviour {
    
    // variables relating to the tutorial conditions
    public TutorialCondition tutorialCondition;
    private bool tutorialConditionMet;

    public string[] initialConditions; // conditions in persistant state data required to run this tutorial, references string key
    private bool [] initialConditionBools; // references value of string key

    public string[] changeConditions; // conditions in persistant state data are changed after tutorial is finished
    
    private bool sceneChecker = false; // checks if this class has been intitialized before

    // Use this for initialization
    void Start () {
        initialConditionBools = new bool[initialConditions.Length];

        bool initChangeCondition = true;

        // check if tutorials were initialized already
        for (int i = 0; i < changeConditions.Length; i++)
        {
            if (!(bool)PersistantStateData.persistantStateData.stateConditions[changeConditions[i]])
            {
                initChangeCondition = false;
            }
        }
        if (initChangeCondition)
        {
            sceneChecker = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!sceneChecker)
        {
            // check every frame if initial conditions have been met
            for (int i = 0; i < initialConditions.Length; i++)
            {
                initialConditionBools[i] = (bool)PersistantStateData.persistantStateData.stateConditions[initialConditions[i]];
            }
            tutorialConditionMet = tutorialCondition.GetCondition();
        }
    }

    // return whether if tutorial is completed to trigger another event
    public bool TutorialConditionMet()
    {
        return tutorialConditionMet;
    }
    public bool GetSceneChecker()
    {
        return sceneChecker;
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
            PersistantStateData.persistantStateData.ChangeStateConditions(changeConditions[i], true);
        }
    }
}
