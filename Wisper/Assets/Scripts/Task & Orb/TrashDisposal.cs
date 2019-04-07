using System.Collections.Generic;
using UnityEngine;

public class TrashDisposal : MonoBehaviour
{

    public List<GameObject> trashAreas = new List<GameObject>(); // List of all trash areas in the scene
    public List<List<GameObject>> trashLists = new List<List<GameObject>>(); // List of trashInArea lists, which will contain all of the trash in the scene
    public List<string> pSDVariables = new List<string>(); // List of PersistentStateVariables
    private SpawnOrbs orbScript; // Local SpawnOrbs script, to create orbs when player cleans trash area
    private int emptyList = -1; // Default value for removing empty lists during play

    // Use this for initialization
    void Start()
    {
        // Find all objects with the TrashArea tag and add them to the trashAreas list
        trashAreas.AddRange(GameObject.FindGameObjectsWithTag("TrashArea"));
        for (int i = 0; i < trashAreas.Count; i++)
        {
            Debug.Log(i);
            // Fill in trashLists and pSDVariables lists with data from the trashArea objects
            trashLists.Add(trashAreas[i].GetComponent<TrashListCreator>().trashInArea);
            pSDVariables.Add(trashAreas[i].GetComponent<TrashListCreator>().pSDVariable);
        }
        // Initialize orbScript to be the local SpawnOrbs component
        orbScript = GetComponent<SpawnOrbs>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // When trash enters the disposal zone, delete it and spawn orbs if it was the only remaining element from a trash area
    private void OnTriggerEnter(Collider other)
    {
        // Trash must have PickUp tag and have "Trash" in its name
        if (other.tag == "PickUp" && other.name.Contains("Trash"))
        {
            // Loop to check if any lists are empty
            // If so, remove said list(s)
            for (int emptyTL = trashLists.Count - 1; emptyTL >= 0; emptyTL--)
            {
                if (trashLists[emptyTL].Count == 0)
                {
                    trashLists.Remove(trashLists[emptyTL]);
                    pSDVariables.Remove(pSDVariables[emptyTL]);
                }
            }

            // Iterate through the trash lists
            for (int tl = 0; tl < trashLists.Count; tl++)
            {
                // Iterate through the elements of each trash list
                for (int tle = 0; tle < trashLists[tl].Count; tle++)
                {
                    // If the name of the trash in the trigger matches the element in the trash list
                    // Remove and destroy it from the trash list it is from
                    if (other.name == trashLists[tl][tle].name)
                    {
                        Debug.Log("Match Found");
                        trashLists[tl].Remove(trashLists[tl][tle]);
                        Destroy(other.gameObject);
                    }
                }
                // If a trash list is empty after destroying a trash, the player has cleaned an area
                // Therefore, spawn orbs for the player and set the corresponding PersistentStateData variable to true
                if (trashLists[tl].Count == 0)
                {
                    Debug.Log("Is Empty");
                    orbScript.DropOrbs();
                    PersistantStateData.persistantStateData.stateConditions[pSDVariables[tl]] = true;
                    // Set emptyList to the index of the now empty list
                    emptyList = tl;
                }
            }
            // If emptyList is not/greater than -1, this means there is a list that is empty
            // Remove the empty list and associated PSD variable from the lists
            if (emptyList > -1)
            {
                trashLists.Remove(trashLists[emptyList]);
                pSDVariables.Remove(pSDVariables[emptyList]);
                Debug.Log("Empty List removed");
                // Set emptyList back to default value
                emptyList = -1;
            }
        }
    }
}