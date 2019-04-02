using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDependentCondition : MonoBehaviour {

    public string dependentCondition;
    private bool doneTutorial = false;

    private TutorialCondition tutorialCondition;

	// Use this for initialization
	void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
    }
	
	// Update is called once per frame
	void Update () {
		if ((bool)PersistantStateData.persistantStateData.stateConditions[dependentCondition] && !doneTutorial)
        {
            tutorialCondition.SetCondition(true);
            doneTutorial = true;
        }
	}
}
