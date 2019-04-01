using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHelpShellsterCondition : MonoBehaviour {

    // Game objects
    public string dependentCondition;
    public Transform swingShellster;
    private Transform player;
    public float proximityDistance;

    private PersistantStateData persistantStateData;
    private bool conditionCheck;
    private float currentDistance;
    
    // variables reliant on distance between shrine and the player
    private float maxDistanceTrigger;
    private float distance;

    private TutorialCondition tutorialCondition;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").transform;
        tutorialCondition = GetComponent<TutorialCondition>();
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
        swingShellster = GameObject.Find("Shellster_Susie").transform;
    }
	
	// Update is called once per frame
	void Update () {

        currentDistance = Vector3.Distance(player.position, swingShellster.position);

        conditionCheck = (bool)persistantStateData.stateConditions[dependentCondition];
        if (currentDistance < proximityDistance && conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
