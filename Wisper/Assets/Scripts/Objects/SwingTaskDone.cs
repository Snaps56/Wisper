using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingTaskDone : MonoBehaviour {

    public GameObject leftSwing;
    public GameObject rightSwing;
    private bool TaskisDone = false;

    private PersistantStateData persistantStateData;

    // Use this for initialization
    void Start () {
        persistantStateData = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float angle = leftSwing.transform.rotation.eulerAngles.y;
        float anglex = leftSwing.transform.rotation.eulerAngles.x;

        if (TaskisDone == false && ((leftSwing.transform.rotation.eulerAngles.x <= 250 && leftSwing.transform.rotation.eulerAngles.x >= 230) || (leftSwing.transform.rotation.eulerAngles.x >= 40 && leftSwing.transform.rotation.eulerAngles.x <= 50)))
        {
            
            // Task is complete
            TaskisDone = true;
            GetComponent<SpawnOrbs>().DropOrbs();
            persistantStateData.stateConditions["SwingTaskDone"] = true;
            persistantStateData.updateCount++;
        }

    }
}
