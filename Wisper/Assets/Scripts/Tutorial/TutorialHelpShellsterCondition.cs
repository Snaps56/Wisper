using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHelpShellsterCondition : MonoBehaviour {

    // Game objects
    public string dependentCondition;
    public Transform shellsterTransform;
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
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            currentDistance = Vector3.Distance(player.position, shellsterTransform.position);
        }
        catch
        {

        }

        conditionCheck = (bool)PersistantStateData.persistantStateData.stateConditions[dependentCondition];
        if (currentDistance < proximityDistance && conditionCheck)
        {
            tutorialCondition.SetCondition(true);
        }
        else
        {
            //Debug.Log(currentDistance);
        }
    }
}
