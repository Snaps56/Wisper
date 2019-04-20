using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameChecker : MonoBehaviour
{
    public string [] taskPSDVariables;
    private bool allTasksDone = false;
    private bool detectedTaskUnfinished = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Tilde))
        {
            allTasksDone = true;
            Debug.Log("Debugged all PSD Tasks. All Tasks Done = " + allTasksDone);
        }
    }
    public bool CheckAllTasksDone()
    {
        detectedTaskUnfinished = false;
        for (int i = 0; i < taskPSDVariables.Length; i++)
        {
            if (!(bool)PersistantStateData.persistantStateData.stateConditions[taskPSDVariables[i]])
            {
                detectedTaskUnfinished = true;
                break;
            }
        }
        if (!detectedTaskUnfinished)
        {
            allTasksDone = true;
        }
        return allTasksDone;
    }
}
