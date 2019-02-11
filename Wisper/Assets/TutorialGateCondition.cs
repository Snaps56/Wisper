using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGateCondition : MonoBehaviour {

    private GameObject gate;
    private bool inRange;
    private PersistantStateData persistantStateData;
    private TutorialCondition tutorialCondition;

    private bool conditionCheck;

    // Use this for initialization
    void Start () {
        gate = GameObject.Find("Gate");
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        inRange = gate.GetComponent<GateTransition>().GetWithinGateRange();
        if (inRange)
        {
            tutorialCondition.SetCondition(true);
        }
	}
}
