using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovementCondition : MonoBehaviour {

    public GameObject player;
    public float distanceTravelRequired;

    private TutorialCondition tutorialCondition;
    private float playerDistanceTraveled;

    // Use this for initialization
    void Start () {
        tutorialCondition = GetComponent<TutorialCondition>();
        playerDistanceTraveled = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerDistanceTraveled += player.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;

        if (playerDistanceTraveled > distanceTravelRequired)
        {
            tutorialCondition.SetCondition(true);
        }
    }
}
