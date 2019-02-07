using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCleanArea : MonoBehaviour {

    public string PSDVariable;
    public bool changesPSD = false;
    public GameObject objectToDisableOnComplete;

    private PersistantStateData psd;
    private bool hasCompleted = false;
    private SpawnOrbs orbScript;

    List<GameObject> objectsWithinArea = new List<GameObject>();

    // Use this for initialization
    void Start () {
        orbScript = GetComponent<SpawnOrbs>();
        //psd = GameObject.Find("PersistantStateData").GetComponent<PersistantStateData>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(objectsWithinArea.Capacity);
        objectsWithinArea.TrimExcess();

        if (objectsWithinArea.Capacity <= 0 && !hasCompleted)
        {
            hasCompleted = true;
            if (changesPSD)
            {
                psd.stateConditions[PSDVariable] = true;
                psd.updateCount++;
            }
            objectToDisableOnComplete.SetActive(false);
            orbScript.DropOrbs();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "PickUp")
        {
            if (!objectsWithinArea.Contains(other.gameObject))
            {
                objectsWithinArea.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PickUp")
        {
            if (objectsWithinArea.Contains(other.gameObject))
            {
                objectsWithinArea.Remove(other.gameObject);
            }
        }
    }
}
