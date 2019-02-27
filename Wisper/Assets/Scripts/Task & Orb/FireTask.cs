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
    private bool TaskisDone = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    //The camera's forward vector
    private Vector3 cameraForward;

    //The vector between the task and camera
    private Vector3 toTask;

    // PTSD
    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start()
    {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    // Update is called once per frame
    void Update () {
        //Update the forward vector
        cameraForward = mainCamera.transform.forward;

        //Find direction between the shrine and camera
        toTask = firepit.transform.position - mainCamera.transform.position;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if ((bool)persistantStateData.stateConditions["ShrineFirstConversationOver"])
        {
            //Player is looking at shrine
            if (Vector3.Dot(cameraForward.normalized, toTask.normalized) > (dotProductAngle) && distance <= deactivateDistance)
            {
                if (TaskisDone == false && ability.GetComponent<ObjectThrow>().GetIsThrowingObjects() == true)
                {
                    // Task is complete
                    TaskisDone = true;
                    GetComponent<SpawnOrbs>().DropOrbs();
                    persistantStateData.stateConditions["FireTaskDone"] = true;
                    persistantStateData.updateCount++;
                    GamePad.SetVibration(playerIndex, 0f, 1f);
                    StartCoroutine(Wait());
                }

            }
        }
    }
}
