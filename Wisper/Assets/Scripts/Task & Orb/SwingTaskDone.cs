using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class SwingTaskDone : MonoBehaviour {

    public GameObject leftSwing;
    public GameObject rightSwing;
    private bool TaskisDone = false;
    bool playerIndexSet = false;
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;


    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start () {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        GamePad.SetVibration(playerIndex, 0f, 0f);
    }

    // Update is called once per frame
    void Update ()
    {
        float angle = leftSwing.transform.rotation.eulerAngles.y;
        float anglex = leftSwing.transform.rotation.eulerAngles.x;

        if((bool)persistantStateData.stateConditions["ShrineFirstConversationOver"])
        {
            if (TaskisDone == false && ((leftSwing.transform.rotation.eulerAngles.x <= 250 && leftSwing.transform.rotation.eulerAngles.x >= 230) || (leftSwing.transform.rotation.eulerAngles.x >= 40 && leftSwing.transform.rotation.eulerAngles.x <= 50)))
            {

                // Task is complete
                TaskisDone = true;
                 GetComponent<SpawnOrbs>().DropOrbs();
                persistantStateData.stateConditions["SwingTaskDone"] = true;
                persistantStateData.updateCount++;
                GamePad.SetVibration(playerIndex, 0f, 1f);
                StartCoroutine(Wait());
            }
        }
    }
}
