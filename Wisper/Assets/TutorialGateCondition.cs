using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGateCondition : MonoBehaviour {

    public GameObject gate;
    private bool inRange;
    private PersistantStateData persistantStateData;
    private TutorialCondition tutorialCondition;

    private bool dependentCondition;

    private bool conditionCheck;
    private bool tutorialDone = false;

    // Use this for initialization
    void Start () {
        //gate = GameObject.Find("Gate");
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        dependentCondition = (bool)persistantStateData.stateConditions["DemoEnd"];
        if (dependentCondition && !tutorialDone && gate.activeSelf)
        {
            inRange = gate.GetComponent<GateTransition>().GetWithinGateRange();
            if (inRange)
            {
                tutorialCondition.SetCondition(true);
                tutorialDone = true;
            }
        }
	}
}
