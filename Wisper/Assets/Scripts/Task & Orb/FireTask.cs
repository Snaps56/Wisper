using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FireTask : MonoBehaviour {


    // "Looking at" Variables
    public GameObject firepit;
    public GameObject player;
    public GameObject ability;
    private bool isThrowing;
    public Camera mainCamera;
    public float dotProductAngle = 0.9f;
    public float deactivateDistance = 10;

    // Vibration Variables
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    //The camera's forward vector
    private Vector3 cameraForward;

    //The vector between the task and camera
    private Vector3 toTask;
	
	// Update is called once per frame
	void Update () {
        //Update the forward vector
        cameraForward = mainCamera.transform.forward;

        //Find direction between the shrine and camera
        toTask = firepit.transform.position - mainCamera.transform.position;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        //Player is looking at shrine
        if (Vector3.Dot(cameraForward.normalized, toTask.normalized) > (dotProductAngle) && distance <= deactivateDistance)
        {
            if (ability.GetComponent<ObjectThrow>().GetIsThrowingObjects() == true)
            Debug.Log("You did it!");
        }
    }
}
