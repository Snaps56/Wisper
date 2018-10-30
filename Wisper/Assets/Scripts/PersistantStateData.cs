using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Centralized location to check for various conditions that determine what should or should not be updated in the game.
// When the instance of this class is accessed, you may use the public hashtable to search conditions.
// Syntax for a search would be stateConditions["Key"], which will return the corresponding value
public class PersistantStateData : MonoBehaviour
{
    public static PersistantStateData persistantStateData;

    public Hashtable stateConditions;   // Hashtable containing key/value pairs of state conditions (probably limited to string/bool pairs).

    // When scene with this loads, initialize the static variable to object with this script if there is none. Object is persistant through scenes.
    // Otherwise if persistantStateData is already loaded into the game/scene, don't overwrite it and delete this object. This enforces singleton status.
    void Awake()
    {
        if (persistantStateData == null)
        {
            DontDestroyOnLoad(gameObject);
            persistantStateData = this;
        }
        else if (persistantStateData != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stateConditions = new Hashtable();
        populateStateConditions();
    }

    // fills the persistantStateConditions with the various conditions. We can consider passing in arguments for initialization when considering save/load functionality.
    public void populateStateConditions()
    {
        stateConditions.Add("ShamusHasHat", false);
        stateConditions.Add("ShrineIsClean", false);
    }
}

