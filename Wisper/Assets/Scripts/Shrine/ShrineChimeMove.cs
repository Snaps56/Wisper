using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineChimeMove : MonoBehaviour {

    public Transform character;

    public float minPlayerDistance;
    public float maxPlayerDistance;

    private float deltaPlayerDistance;
    private float currentPlayerDistance;
    private float currentPercentPlayerDistance;

    public float floatHeightRange;

    private float currentFloatHeight;
    private float minFloatHeight;
    private float maxFloatHeight;
    private Vector3 initPosition;

    // Use this for initialization
    void Start () {
        deltaPlayerDistance = maxPlayerDistance - minPlayerDistance;

        initPosition = transform.position;
        minFloatHeight = initPosition.y;
        maxFloatHeight = minFloatHeight + floatHeightRange;
        currentFloatHeight = minFloatHeight;
        currentPlayerDistance = Vector3.Distance(character.position, transform.position);

        LiftChime();
    }
	
	// Update is called once per frame
	void Update () {
        currentPlayerDistance = Vector3.Distance(character.position, transform.position);

        //Debug.Log("Min " + minPlayerDistance + ", Max " + maxPlayerDistance + ", Current " + currentPlayerDistance + ", Percent " + currentPercentPlayerDistance + ", Float Height " + currentFloatHeight);

        currentPercentPlayerDistance = (currentPlayerDistance - minPlayerDistance) / maxPlayerDistance;

        if (currentPercentPlayerDistance > 0)
        {
            LiftChime();
        }
	}
    void LiftChime()
    {
        currentFloatHeight = -currentPercentPlayerDistance * floatHeightRange;
        Vector3 finalFloatVector = initPosition;
        finalFloatVector.y = initPosition.y + currentFloatHeight;
        transform.position = finalFloatVector;
    }
}
