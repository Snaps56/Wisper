using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashListCreator : MonoBehaviour
{

    public List<GameObject> trashInArea = new List<GameObject>(); // List of trash that belongs to the area
    public string pSDVariable; // Name of the trash area's PersistentStateData variable, used to determine if corresponding trash should exist in the scene
    public string trashName;

    // Use this for initialization
    void Start()
    {
        // Trash exists in scene at first
        // If the player has already cleaned the trash in the area
        // destroy the trash and clear the trashInArea list

        List<GameObject> gameObjs = new List<GameObject>();
        gameObjs.AddRange(GameObject.FindGameObjectsWithTag("PickUp"));

        foreach (GameObject gameObj in gameObjs) {
            if (gameObj.name.Contains(trashName)) {
                trashInArea.Add(gameObj);
            }
        }

        gameObjs.Clear();

        if ((bool)PersistantStateData.persistantStateData.stateConditions[pSDVariable])
        {
            Debug.Log("Destroy all objects");
            for (int i = 0; i < trashInArea.Count; i++)
            {
                Destroy(trashInArea[i]);
            }
            trashInArea.Clear();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}