using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnoughOrbs : MonoBehaviour
{
    private bool doneTutorial = false;

    private TutorialCondition tutorialCondition;

    // Use this for initialization
    void Start()
    {
        tutorialCondition = GetComponent<TutorialCondition>();
    }

    // Update is called once per frame
    void Update()
    {
        if(((float)PersistantStateData.persistantStateData.stateConditions["TotalOrbsDeposited"] + (float)PersistantStateData.persistantStateData.stateConditions["OrbCount"])
            >= (float)PersistantStateData.persistantStateData.stateConditions["TotalOrbCount"])
        {
            PersistantStateData.persistantStateData.ChangeStateConditions("HasEnoughOrbsEndgame", true);
        }
        if ((bool) PersistantStateData.persistantStateData.stateConditions["TutorialPlayerEnterCity"] && (bool)PersistantStateData.persistantStateData.stateConditions["HasEnoughOrbsEndgame"])
        {
            tutorialCondition.SetCondition(true);
            doneTutorial = true;
        }
    }
}
